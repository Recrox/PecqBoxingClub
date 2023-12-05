using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RamDam.BackEnd.Core.Models.Api
{
    public class Favorites : BaseApiObject<Table.Favorites, Guid>
    {
        public User User { get; set; }
        public Movie Movie { get; set; }
        public Scheduling Scheduling { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
    }

    public class FavoriteProfile : Profile
    {
        public FavoriteProfile()
        {
            CreateMap<Favorites, Table.Favorites>()
                .ForMember(u => u.User, cfg => cfg.Ignore())
                .ForMember(u => u.Movie, cfg => cfg.Ignore())
                .ForMember(u => u.Scheduling, cfg => cfg.Ignore())
                .ForMember(u => u.IdUser, cfg => cfg.MapFrom(u => u.User.Id))
                .ForMember(u => u.IdMovie, cfg => cfg.MapFrom(u => u.Movie.Id))
                .ForMember(u => u.IdScheduling, cfg => cfg.MapFrom(u => u.Scheduling.Id));
        }
    }

}
