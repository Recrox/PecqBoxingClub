using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RamDam.BackEnd.Core.Models.Api
{

    public class SocialNetwork : BaseApiObject<Table.SocialNetwork, Guid>
    {

        public string Social { get; set; }

    }

    public class SocialNetworkProfile : Profile
    {
        public SocialNetworkProfile()
        {
            CreateMap<SocialNetwork, Table.SocialNetwork>();
        }
    }

}
