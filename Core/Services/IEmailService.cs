using RamDam.BackEnd.Core.Models.Table;
using System.Threading.Tasks;

namespace RamDam.BackEnd.Core.Services
{
    public interface IEmailService
    {
        Task SendPasswordResetAsync(User user, string link);
        Task SendWelcomeAsync(User user, string link);
    }
}
