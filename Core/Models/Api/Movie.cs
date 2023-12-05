using AutoMapper;
using FluentValidation;
using System;
using System.Collections.Generic;

namespace RamDam.BackEnd.Core.Models.Api
{
    public class Movie : BaseApiObject<Table.Movie, Guid>
    {
        public string Title { get; set; }
        public string Realisator { get; set; }
        public string Length { get; set; }
        public string Description { get; set; }
        public string Attach { get; set; }
        public string Trailer { get; set; }

        public string Link { get; set; }
        public long? IdRamDamMovie { get; set; }

        public bool CanVote { get; set; }

        public List<Scheduling> Scheduling { get; set; }

        public List<SubMovie> Submovies { get; set; }

    }

    public class MovieProfile : Profile
    {
        public MovieProfile()
        {
            CreateMap<Movie, Table.Movie>()
                .ForMember(m => m.IdRamDamMovie, cfg => cfg.MapFrom(m => m.IdRamDamMovie))
                .ForMember(m => m.Length, cfg => cfg.MapFrom(m => m.Length))
                .ForMember(m => m.Realisator, cfg => cfg.MapFrom(m => m.Realisator))
                .ForMember(m => m.Attach, cfg => cfg.MapFrom(m => m.Attach))
                .ForMember(m => m.Description, cfg => cfg.MapFrom(m => m.Description))
                .ForMember(m => m.Link, cfg => cfg.MapFrom(m => m.Link))
                .ForMember(m => m.Scheduling, cfg => cfg.Ignore())
                .ForMember(m => m.Trailer, cfg => cfg.MapFrom(m => m.Trailer))
                .ForMember(m => m.Title, cfg => cfg.MapFrom(m => m.Title));
        }
    }



    public class MovieValidator : AbstractValidator<Movie>
    {
        public MovieValidator()
        {
            //RuleFor(u => u.Description).MaximumLength(100);
        }
    }
}
