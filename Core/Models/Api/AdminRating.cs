using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RamDam.BackEnd.Core.Models.Api
{
    public class AdminRating
    {
        public string Image { get; set; }
        public Guid MovieId { get; set; }
        public string Title { get; set; }
        public double RatingsAverage { get; set; }
        public double DisturbingAverage { get; set; }
        public int RatingsNumber { get; set; }
        public List<AdminRating> SubMovies { get; set; }
    }
}
