using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RamDam.BackEnd.Core.Models.Api
{
    public class MovieAdmin
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Attach { get; set; }

        public List<MovieAdmin> SubMovies { get; set; }
        public bool CanVote { get; set; }
    }

}
