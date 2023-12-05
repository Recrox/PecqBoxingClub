using RamDam.BackEnd.Core.Models.Table;
using System.Threading.Tasks;

namespace RamDam.BackEnd.Core.Services
{
    public interface IPasswordResetService
    {
        Task SendPasswordWelcomeAsync(User user, bool isFirstAdmin = false);
        Task SendPasswordResetAsync(User user);
        Task<string> BuildPasswordResetLink(User user, string baseUrl);
    }
}
