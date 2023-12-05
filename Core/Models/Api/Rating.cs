using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RamDam.BackEnd.Core.Models.Api
{
    public class Rating : BaseApiObject<Table.Rating, Guid>
    {

        public User? User { get; set; }

        public Movie Movie { get; set; }

        public int? Note { get; set; }

        public int? DisturbingRate { get; set; }
        
        public int? Answer { get; set; }

    }

    public class RatingProfile : Profile
    {
        public RatingProfile()
        {
            CreateMap<Rating, Table.Rating>()
                .ForMember(u => u.User, cfg => cfg.Ignore())
                .ForMember(u => u.Movie, cfg => cfg.Ignore())
                .ForMember(u => u.IdUser, cfg => cfg.MapFrom(u => u.User.Id))
                .ForMember(u => u.IdMovie, cfg => cfg.MapFrom(u => u.Movie.Id));

        }
    }

}
