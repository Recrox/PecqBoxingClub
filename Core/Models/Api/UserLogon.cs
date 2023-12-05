using AutoMapper;
using RamDam.BackEnd.Core.Models.Api;
using System;

namespace RamDam.BackEnd.Core.Models.Api
{
    public class UserLogon : BaseApiObject<Table.UserLogon, Guid>
    {
        public Guid UserId { get; set; }
        public DateTime Date { get; set; }
    }

    public class UserLogonProfile : Profile
    {
        public UserLogonProfile()
        {
            CreateMap<UserLogon, Table.UserLogon>();
        }
    }
}
