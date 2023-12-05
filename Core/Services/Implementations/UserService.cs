using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using RamDam.BackEnd.Core.Repositories;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using RamDam.BackEnd.Core.Models.Table;
using AutoMapper;

namespace RamDam.BackEnd.Core.Services
{
    public class UserService : UserManager<User>, IUserService
    {
        private readonly IIdentityUserRepository _identityUserRepository;
        private readonly IUserRepository _userRepository;
        private readonly IPasswordResetService _passwordResetService;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;
        private readonly CurrentContext _currentContext;

        public UserService(
            IIdentityUserRepository identityUserRepository,
            IUserRepository userRepository,
            IUserStore<User> store,
            IOptions<IdentityOptions> optionsAccessor,
            IPasswordHasher<User> passwordHasher,
            IEnumerable<IUserValidator<User>> userValidators,
            IEnumerable<IPasswordValidator<User>> passwordValidators,
            ILookupNormalizer keyNormalizer,
            IdentityErrorDescriber errors,
            IServiceProvider services,
            ILogger<UserManager<User>> logger,
            IEmailService emailService,
            IPasswordResetService passwordResetService,
            IMapper mapper,
            CurrentContext currentContext)
            : base(
                  store,
                  optionsAccessor,
                  passwordHasher,
                  userValidators,
                  passwordValidators,
                  keyNormalizer,
                  errors,
                  services,
                  logger)
        {
            _identityUserRepository = identityUserRepository;
            _userRepository = userRepository;
            _mapper = mapper;
            _currentContext = currentContext;
            _emailService = emailService;
            _passwordResetService = passwordResetService;
        }

        public async Task<User> GetUserByPrincipalAsync(ClaimsPrincipal principal)
        {
            var userId = GetProperUserId(principal);
            if (!userId.HasValue)
            {
                return null;
            }

            return await GetUserByIdAsync(userId.Value);
        }

        public Guid? GetProperUserId(ClaimsPrincipal principal)
        {
            if (!Guid.TryParse(GetUserId(principal), out var userIdGuid))
            {
                return null;
            }

            return userIdGuid;
        }

        public async Task<User> GetUserByIdAsync(Guid userId)
        {
            if (_currentContext?.User != null && _currentContext.User.Id == userId)
            {
                return _currentContext.User;
            }

            return await _identityUserRepository.GetJoinedUserById(userId);
        }

        public async Task<Models.Api.User> SaveAsync(Models.Api.User userRequest)
        {
            Models.Api.User user = null;
            Models.Api.User userByMail = null;
            if (!userRequest.SocialNetwork.Id.Equals(Models.Api.SocialNetworkEnum.Email))
                userByMail = await _userRepository.GetSingleAsync(
                    u => u.UserName.Equals(userRequest.IdSocial.Normalize().ToLowerInvariant()));
            else
                 userByMail = await _userRepository.GetSingleAsync(
                    u => u.UserName == userRequest.UserName.Normalize().ToLowerInvariant());

            if (userRequest.Id.Equals(default)) // = Creation d'un nouvel utilisateur
            {
                var identityUser = new User();
                if (userByMail == null)
                {
                    var result = await CreateAsync(userRequest.ToTableObject(_mapper.Map(userRequest, identityUser), _currentContext));
                    if (result.Succeeded)
                    {
                        if (userRequest?.SocialNetwork?.Id == null || userRequest.SocialNetwork.Id.Equals(Models.Api.SocialNetworkEnum.Email))
                            await _passwordResetService.SendPasswordWelcomeAsync(identityUser);
                        if (!string.IsNullOrEmpty(userRequest.Password))
                            result = await AddPasswordAsync(identityUser, userRequest.Password);
                    }
                    user = await _userRepository.GetByIdAsync(identityUser.Id);
                }                
            }
            else // Mise à jour d'un utilisateur existant
            {
                
                if (userByMail == null || userByMail.Id == userRequest.Id)
                {
                    if (!string.IsNullOrEmpty(userRequest.Password))
                    {
                        var identityUser = await FindByNameAsync(userRequest.Email);
                        await RemovePasswordAsync(identityUser);
                        await AddPasswordAsync(identityUser, userRequest.Password);
                    }
                    user = await _userRepository.ReplaceAsync(userRequest, reloadResult: true);
                }
            }
            return user;
        }
        
        public async Task<bool> DeleteAsync(Guid userId)
        {
            var result = await ChangeActiveAsync(userId, false);
            return result;
        }

        public async Task<bool> UndeleteAsync(Guid userId)
        {
            var result = await ChangeActiveAsync(userId, true);
            return result;
        }


        public async Task<bool> SendPasswordResetAsync(string Email)
        {
            var user = await FindByNameAsync(Email);
            if (user != null && user.IdSocialNetwork.Equals(Models.Api.SocialNetworkEnum.Email))
            {
                await _passwordResetService.SendPasswordResetAsync(user);
                return true;
            }
            return false;
        }

        public async Task<bool> SendPasswordInitAsync(string Email)
        {
            var user = await FindByNameAsync(Email);
            if (user != null)
            {
                await _passwordResetService.SendPasswordWelcomeAsync(user);
                return true;
            }
            return false;
        }

        public async Task<bool> ResetPasswordAsync(Guid userId, string token, string newPassword)
        {
            var user = await FindByIdAsync(userId.ToString());
            if (user != null)
            {
                var result = await ResetPasswordAsync(user, token, newPassword);
                if (result.Succeeded)
                    return true;
            }
            return false;
        }

        private async Task<bool> ChangeActiveAsync(Guid userId, bool isActive)
        {
            var user = await _identityUserRepository.GetByIdAsync(userId);
            if (user == null)
                return false;

            user.IsActive = isActive;
            await _identityUserRepository.UpsertAsync(user);

            return true;
        }


    }
}
