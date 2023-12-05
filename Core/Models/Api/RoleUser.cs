using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RamDam.BackEnd.Core.Models.Api
{
    public class RoleUser: BaseApiObject<Table.RoleUser, Guid>
    {
        public User User { get; set; }
        public Role Role { get; set; }
    }

    public class RoleUserProfile : Profile
    {
        public RoleUserProfile()
        {
            CreateMap<RoleUser, Table.RoleUser>()
                .ForMember(u => u.User, cfg => cfg.Ignore())
                .ForMember(u => u.Role, cfg => cfg.Ignore())
                .ForMember(u => u.IdUser, cfg => cfg.MapFrom(u => u.User.Id))
                .ForMember(r => r.IdRole, cfg => cfg.MapFrom(r => r.Role.Id));
        }
    }
}
