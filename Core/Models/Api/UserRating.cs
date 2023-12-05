using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RamDam.BackEnd.Core.Models.Api
{
    public class UserRating
    {
        public Guid IdRating { get; set; }
        public Guid IdMovie { get; set; }
        public int Rate { get; set; }
        public int DisturbingRate { get; set; }
        public int Answer { get; set; }
        public string Title { get; set; }
        public string Image { get; set; }
        public string Realisator { get; set; }
    }
}
