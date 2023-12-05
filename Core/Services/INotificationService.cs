using RamDam.BackEnd.Core.Models.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RamDam.BackEnd.Core.Services
{
    public interface INotificationService
    {
        Task<Notification> SendNotification(Notification notification);
        Task<Notification> SendNotificationNow(Notification notification);
        void CancelNotification(long jobId);
        Task<bool> DeleteNotification(Guid notificationId);
    }
}
