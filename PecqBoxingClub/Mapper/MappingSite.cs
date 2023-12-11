namespace Site.Mapper;

using AutoMapper;
using PecqBoxingClubApi.BackEnd.Core.Models.Table;

public class MappingSite : Profile
{
    public MappingSite()
    {
        CreateMap<Member, Core.Models.Api.Member>().ReverseMap();
        CreateMap<License, Core.Models.Api.License>().ReverseMap();
    }
}