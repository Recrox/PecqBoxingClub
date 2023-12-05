using RamDam.BackEnd.Core.Models.Mail;
using System.Threading.Tasks;

namespace RamDam.BackEnd.Core.Services
{
    public interface IEmailDeliveryService
    {
        Task SendEmailAsync(MailMessage message);
    }
}
