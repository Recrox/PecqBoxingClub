using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using Hangfire;
using RamDam.BackEnd.Core.Models.Api;
using RamDam.BackEnd.Core.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RamDam.BackEnd.Core.Services
{
    public class NotificationService: INotificationService
    {
        private readonly INotificationRepository _notificationRepository;

        public NotificationService(INotificationRepository notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }

        public void CancelNotification(long jobId)
        {
            BackgroundJob.Delete(jobId.ToString());
        }

        public async Task<bool> DeleteNotification(Guid notificationId)
        {
            var notifToDelete = await _notificationRepository.GetByIdAsync(notificationId);
            if (notifToDelete != null)
            {
                this.CancelNotification(notifToDelete.IdJob);
                return await _notificationRepository.DeleteAsync(notificationId);
            }
            return false;
        }

        public async Task<Models.Api.Notification> SendNotification(Models.Api.Notification notification)
        {
            var message = new Message()
            {
                Notification = new FirebaseAdmin.Messaging.Notification
                {
                    Title = notification.Title,
                    Body = notification.Body
                },
                Topic = notification.Topic
            };
          
            //var jobId = BackgroundJob.Schedule(() => SendNotification(message), TimeZone.CurrentTimeZone.ToLocalTime(notification.SendDate) - DateTime.Now);
            var jobId = BackgroundJob.Schedule(() => SendNotification(message),(notification.SendDate.ToUniversalTime() - DateTime.Now.ToUniversalTime()) );

            notification.IdJob = int.Parse(jobId);
            var result = await _notificationRepository.CreateAsync(notification);
            return result;
        }
        public void SendNotification(Message message)
        {
            _ = Task.Run(async () =>
            {
                if (FirebaseApp.DefaultInstance == null)
                {
                    var defaultApp = FirebaseApp.Create(new AppOptions()
                    {
                        Credential = GoogleCredential.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ramdamapp-firebase-adminsdk-8sesu-3ee0d8b00d.json"))
                    });
                }
                var messaging = FirebaseMessaging.DefaultInstance;
                await messaging.SendAsync(message);
            });

        }

        public async Task<Models.Api.Notification> SendNotificationNow(Models.Api.Notification notification)
        {
            var message = new Message()
            {
                Notification = new FirebaseAdmin.Messaging.Notification
                {
                    Title = notification.Title,
                    Body = notification.Body
                },
                Topic = notification.Topic
            };
            await SendNotificationNow(message);
            notification.SendDate = DateTime.Now;
            var result = await _notificationRepository.CreateAsync(notification);
            return result;
        }

        public async Task SendNotificationNow(Message message)
        {
            if (FirebaseApp.DefaultInstance == null)
            {
                var defaultApp = FirebaseApp.Create(new AppOptions()
                {
                    Credential = GoogleCredential.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ramdamapp-firebase-adminsdk-8sesu-3ee0d8b00d.json"))
                });
            }
            var messaging = FirebaseMessaging.DefaultInstance;
            await messaging.SendAsync(message);
        }
    }
}
