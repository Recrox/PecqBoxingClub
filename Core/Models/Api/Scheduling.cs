using AutoMapper;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RamDam.BackEnd.Core.Models.Api
{
    public class Scheduling : BaseApiObject<Table.Scheduling, Guid>
    {
        public Movie Movie { get; set; }
        public DateTime StartTime { get; set; }
        public long? IdJob { get; set; }
        public bool TeamAttendance { get; set; }

    }



    public class SchedulingProfile : Profile
    {
        public SchedulingProfile()
        {
            CreateMap<Scheduling, Table.Scheduling>()
                
                .ForMember(u => u.Movie, cfg => cfg.Ignore())
                .ForMember(u => u.IdMovie, cfg => cfg.MapFrom(u => u.Movie.Id));

        }
    }

}
