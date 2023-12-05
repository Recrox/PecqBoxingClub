using System;

namespace RamDam.BackEnd.Core.Models.Api
{
    public class GenericUserRating
    {
        public Guid IdSubMovie { get; set; }
        public Guid IdParentMovie { get; set; } 
        public User? User { get; set; }
        public int Rate { get; set; }
        public int DisturbingRate { get; set; }
    }
}
