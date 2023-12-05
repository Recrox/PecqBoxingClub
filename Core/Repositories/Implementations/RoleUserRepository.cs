using AutoMapper;
using RamDam.BackEnd.Core.Models.Table;
using Sieve.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RamDam.BackEnd.Core.Repositories.Implementations
{
    public class RoleUserRepository : Repository<RoleUser, Models.Api.RoleUser, Guid>, IRoleUserRepository
    {
        public RoleUserRepository(
            RamDamContext context,
            CurrentContext currentContext,
            IMapper mapper,
            ISieveProcessor sieveProcessor) : base(context, currentContext, mapper, sieveProcessor)
        {

        }

        protected override Expression<Func<RoleUser, Models.Api.RoleUser>> Select => ru => new Models.Api.RoleUser
        {
            Role = new Models.Api.Role
            {
                RoleName = ru.Role.RoleName,
                Id = ru.Id
            },
            User = new Models.Api.User
            {
                Email = ru.User.Email,
                UserName = ru.User.UserName
            }
        };
    }
}
