using HandlebarsDotNet;
using RamDam.BackEnd.Configuration;
using RamDam.BackEnd.Core.Models.Mail;
using RamDam.BackEnd.Core.Models.Table;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RamDam.BackEnd.Core.Services
{
    public class EmailService : IEmailService
    {
        private const string TemplatePath = "Templates/Email";

        private readonly IEmailDeliveryService _emailDeliveryService;
        private readonly GlobalSettings _globalSettings;
        private readonly Dictionary<string, Func<object, string>> _templateCache;

        public EmailService(
            GlobalSettings globalSettings,
            IEmailDeliveryService emailDeliveryService)
        {
            _emailDeliveryService = emailDeliveryService;
            _globalSettings = globalSettings;
            _templateCache = new Dictionary<string, Func<object, string>>();
        }

        public async Task SendPasswordResetAsync(User user, string link)
        {
            var message = await CreateMailMessageAsync("reset_password", new BaseMailModel
            {
                Url = link,
                UserName = user.UserName,
            }, user);
            await SendEmail(message);
        }
        public async Task SendWelcomeAsync(User user, string link)
        {
            var message = await CreateMailMessageAsync("welcome", new BaseMailModel
            {
                Url = link,
                UserName = user.UserName,
            }, user);
            await SendEmail(message);
        }
       

        private async Task SendEmail(MailMessage message)
        {
            await _emailDeliveryService.SendEmailAsync(message);
        }

        private async Task<MailMessage> CreateMailMessageAsync(string templateName, BaseMailModel data, User user)
        {
            data.Name = user.Email;
            return await CreateMailMessageAsync(templateName, data, new List<MailAddress>
            {
                new MailAddress
                {
                    Email = user.Email,
                    Name = user.UserName
                }
            });
        }

        private async Task<MailMessage> CreateMailMessageAsync(string templateName, BaseMailModel data, List<MailAddress> to)
        {
            var languageCode = "fr";
            data.ProjectName = _globalSettings.ProjectName;
            var msg = new MailMessage
            {
                Subject = _globalSettings.Mail.Subjects[$"{templateName}_{languageCode}"],
                Language = languageCode,
                From = new MailAddress
                {
                    Name = _globalSettings.Mail.SenderName,
                    Email = _globalSettings.Mail.SenderEmail
                },
                ReplyTo = _globalSettings.Mail.ReplyEmail,
                To = to,
                HtmlContent = await RenderAsync($"{templateName}.html", languageCode, data),
                TextContent = await RenderAsync($"{templateName}.txt", languageCode, data)
            };

            return msg;
        }

        private async Task<string> RenderAsync(string templateName, string languageCode, BaseMailModel data)
        {
            var key = $"{languageCode}_{templateName}";
            if (!_templateCache.TryGetValue(key, out var template))
            {
                var source = await ReadSourceAsync(templateName);
                if (source != null)
                {
                    template = Handlebars.Compile(source);
                    _templateCache.Add(key, template);
                }
            }
            return template != null ? template(data) : null;
        }

        private async Task<string> ReadSourceAsync(string templateName)
        {
            var template = $"{TemplatePath}/{templateName}";
            if (System.IO.File.Exists(template))
            {
                using (var reader = System.IO.File.OpenText(template))
                {
                    return await reader.ReadToEndAsync();
                }
            }
            return null;
        }
    }
}
