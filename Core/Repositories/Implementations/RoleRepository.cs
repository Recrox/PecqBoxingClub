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
    public class RoleRepository : Repository<Role, Models.Api.Role, Guid>, IRoleRepository
    {
        public RoleRepository(
            RamDamContext context,
            CurrentContext currentContext,
            IMapper mapper,
            ISieveProcessor sieveProcessor) : base(context, currentContext, mapper, sieveProcessor)
        {

        }

        protected override Expression<Func<Role, Models.Api.Role>> Select => r => new Models.Api.Role
        {
            Id = r.Id,
            RoleName = r.RoleName
        };
    }
}
