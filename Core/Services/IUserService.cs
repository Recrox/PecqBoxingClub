using RamDam.BackEnd.Core.Models.Table;
using Microsoft.AspNetCore.Identity;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace RamDam.BackEnd.Core.Services
{
    public interface IUserService
    {
        Task<User> GetUserByPrincipalAsync(ClaimsPrincipal principal);
        Task<Models.Api.User> SaveAsync(Models.Api.User userRequest);
        Task<bool> DeleteAsync(Guid userId);
        Task<bool> UndeleteAsync(Guid userId);
        Task<IdentityResult> ChangePasswordAsync(User user, string currentPassword, string newPassword);
        Task<bool> SendPasswordResetAsync(string Email);
        Task<bool> SendPasswordInitAsync(string Email);
        Task<bool> ResetPasswordAsync(Guid userId, string token, string newPassword);
    }
}
