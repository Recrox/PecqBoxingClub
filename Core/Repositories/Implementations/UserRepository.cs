using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using RamDam.BackEnd.Core.Models.Table;
using Sieve.Services;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using RamDam.BackEnd.Core.Models.SP;

namespace RamDam.BackEnd.Core.Repositories
{
    public class UserRepository : Repository<User, Models.Api.User, Guid>, IUserRepository
    {
        public UserRepository(
            RamDamContext context,
            CurrentContext currentContext,
            ISieveProcessor sieveProcessor,
            IMapper mapper)
            : base(context, currentContext, mapper, sieveProcessor)
        {
        }

        protected override Task<User> GetTableObjectAsync(Guid id)
        {
            return _context.Users
                .Where(c => c.Id == id)
                .Include(u => u.Roles)
                .SingleOrDefaultAsync();
        }

        protected override Expression<Func<User, Models.Api.User>> Select => u => new Models.Api.User
        {
            Id = u.Id,
            Email = u.Email,
            UserName = u.UserName,
            IsActive = u.IsActive,
            Password = u.Password,
            IdSocial = u.IdSocial,
            IsAdmin = u.IsAdmin,
            WantsNewsletters = u.WantsNewsLetters,
            SocialNetwork = new Models.Api.SocialNetwork
            {
               Id = u.SocialNetwork.Id,
               Social = u.SocialNetwork.Social
            },
            Roles = u.Roles.Select(r => new Models.Api.Role() { RoleName = r.Role.RoleName, Id = r.Role.Id }).ToList()
        };
    }

    public class IdentityUserRepository : IdentityRepository<User, Guid>, IIdentityUserRepository
    {
        public IdentityUserRepository(RamDamContext context, CurrentContext currentContext)
            : base(context, currentContext)
        {
        }

        public async Task<User> GetByNormalizedUserNameAsync(string normalizedUsername)
        {
            return await GetSingleAsync(u => u.UserName.ToLower() == normalizedUsername);
        }

        protected override Expression<Func<User, User>> Select => u => new User
        {
            Id = u.Id,
            Email = u.Email,
            IdSocialNetwork = u.IdSocialNetwork,
            Password = u.Password,
            IsActive = u.IsActive,
            UserName = u.UserName,
            IdSocial = u.IdSocial,
            IsAdmin = u.IsAdmin,
            WantsNewsLetters = u.WantsNewsLetters,
            Roles = u.Roles
        };

        public virtual async Task<User> GetJoinedUserById(Guid id)
        {
            return await (from e in _context.Set<User>()
                          where e.Id.Equals(id)
                          select e)
                          .Include(u => u.Roles)
                           .ThenInclude(r => r.Role)
                          .SingleOrDefaultAsync();
        }
    }
}
