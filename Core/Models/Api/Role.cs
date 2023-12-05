using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RamDam.BackEnd.Core.Models.Api
{
    public class Role : BaseApiObject<Table.Role, Guid>
    {
        public Guid Id { get; set; }
        public string RoleName { get; set; }
    }

    public class RoleProfile : Profile
    {
        public RoleProfile()
        {
            CreateMap<Role, Table.Role>()
                .ForMember(R => R.Id, cfg => cfg.MapFrom(r => r.Id))
                .ForMember(R => R.RoleName, cfg => cfg.MapFrom(r => r.RoleName));
        }
    }
}
