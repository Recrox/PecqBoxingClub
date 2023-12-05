using System;
using System.Linq;
using System.Threading.Tasks;
using RamDam.BackEnd.Configuration;
using RamDam.BackEnd.Core.Models.Mail;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using MimeKit;

namespace RamDam.BackEnd.Core.Services
{
    public class SmtpEmailDeliveryService : IEmailDeliveryService
    {
        private readonly ILogger<SmtpEmailDeliveryService> _logger;
        private readonly GlobalSettings _globalSettings;

        public SmtpEmailDeliveryService(ILogger<SmtpEmailDeliveryService> logger, GlobalSettings globalSettings)
        {
            _logger = logger;
            _globalSettings = globalSettings;
        }

        public async Task SendEmailAsync(MailMessage mail)
        {
            if (mail.To.Count == 0)
                return;
            try
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(mail.From.Name, mail.From.Email));
                message.To.AddRange(mail.To.Select(to => new MailboxAddress(to.Name, to.Email)));
                message.ReplyTo.Add(new MailboxAddress(mail.ReplyTo));
                message.Subject = mail.Subject;

                var builder = new BodyBuilder();
                if (!string.IsNullOrWhiteSpace(mail.TextContent))
                    builder.TextBody = mail.TextContent;
                if (!string.IsNullOrWhiteSpace(mail.HtmlContent))
                    builder.HtmlBody = mail.HtmlContent;
                message.Body = builder.ToMessageBody();

                using (var client = new SmtpClient())
                {
                    await client.ConnectAsync(_globalSettings.Mail.Smtp.Host, (int)_globalSettings.Mail.Smtp.Port, SecureSocketOptions.None);
                    await client.SendAsync(message);
                    await client.DisconnectAsync(true);
                }
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "send mail error");
            }
        }
    }
}
