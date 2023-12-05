using RamDam.BackEnd.Core.Models.Table;
using System;
using System.Threading.Tasks;

namespace RamDam.BackEnd.Core.Repositories
{
    public interface IUserRepository : IRepository<User, Models.Api.User, Guid>
    {
    }

    public interface IIdentityUserRepository : IRepository<User, Guid>
    {
        Task<User> GetByNormalizedUserNameAsync(string normalizedUsername);
        Task<User> GetJoinedUserById(Guid id);
        //Task<Models.Api.User> GetUserWithRole(Guid userId);
    }
}
