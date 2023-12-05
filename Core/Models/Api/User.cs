using AutoMapper;
using FluentValidation;
using RamDam.BackEnd.Core.Models.Api;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace RamDam.BackEnd.Core.Models.Api
{
    public class User : BaseApiObject<Table.User, Guid>
    {
        public string? UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    
        public bool IsActive { get; set; }
        public string IdSocial { get; set; }
        public bool IsAdmin { get; set; }
        public bool WantsNewsletters { get; set; }

        public List<Role> Roles { get; set; }
        public SocialNetwork SocialNetwork { get; set; }

    }

   

    public class UserValidator : AbstractValidator<User>
    {
        public UserValidator()
        {
           // RuleFor(u => u.Email).NotEmpty().EmailAddress().MaximumLength(100);
        }
    }

    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, Table.User>()
                .ForMember(u => u.UserName, cfg => cfg.MapFrom(u => u.UserName))
                .ForMember(u => u.Password, cfg => cfg.Ignore())
                .ForMember(u => u.IdSocial, cfg => cfg.MapFrom(u => u.IdSocial))
                .ForMember(u => u.SocialNetwork, cfg => cfg.Ignore())
                .ForMember(u => u.IdSocialNetwork, cfg => cfg.MapFrom(u => u.SocialNetwork.Id))
                .ForMember(u => u.IsActive, cfg => cfg.MapFrom(u => u.IsActive))
                .ForMember(u => u.IsAdmin, cfg => cfg.MapFrom(u => u.IsAdmin))
                .ForMember(u => u.WantsNewsLetters, cfg => cfg.MapFrom(u => u.WantsNewsletters))
                .ForMember(u => u.Roles, cfg => cfg.MapFrom(u => u.Roles));
            
        }
    }
}
