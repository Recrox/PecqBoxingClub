using System;
using System.Threading.Tasks;
using System.Web;
using RamDam.BackEnd.Configuration;
using RamDam.BackEnd.Core.Models.Table;
using Microsoft.AspNetCore.Identity;

namespace RamDam.BackEnd.Core.Services
{
    public class PasswordResetService : IPasswordResetService
    {
        private readonly GlobalSettings _globalSettings;
        private readonly CurrentContext _currentContext;
        private UserManager<User> _userManager;
        private readonly IEmailService _emailService;

        public PasswordResetService(
            GlobalSettings globalSettings,
            CurrentContext currentContext,
            UserManager<User> userManager,
            IEmailService emailService
            )
        {
            _globalSettings = globalSettings;
            _currentContext = currentContext;
            _userManager = userManager;
            _emailService = emailService;
        }

        public async Task SendPasswordWelcomeAsync(User user, bool isFirstAdmin = false)
        {
            var link = await BuildPasswordResetLink(user, _globalSettings.WebSiteUri.Welcome);
            await _emailService.SendWelcomeAsync(user, link);
        }

        public async Task SendPasswordResetAsync(User user)
        {
            var link = await BuildPasswordResetLink(user, _globalSettings.WebSiteUri.ChangePassword);
            await _emailService.SendPasswordResetAsync(user, link);
        }

        public async Task<string> BuildPasswordResetLink(User user, string baseUrl)
        {
            DateTimeOffset expiration = new DateTimeOffset(_currentContext.DateTime);

            expiration = expiration.AddMinutes(GetTokenExpirationInMinutes());

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var builder = new UriBuilder(baseUrl)
            {
                Port = -1
            };
            var query = HttpUtility.ParseQueryString(builder.Query);
            query["token"] = token;
            query["user"] = user.UserName;
            query["id"] = user.Id.ToString();
            query["exp"] = expiration.ToString();
            builder.Query = query.ToString();
            return builder.ToString();
        }

        public uint GetTokenExpirationInMinutes()
        {
            if (_globalSettings != null
                && _globalSettings.Identity != null
                && _globalSettings.Identity.DataProtectorTokenLifeSpan.HasValue)
            {
                return _globalSettings.Identity.DataProtectorTokenLifeSpan.Value;
            }
            return 1440;
        }
    }
}
