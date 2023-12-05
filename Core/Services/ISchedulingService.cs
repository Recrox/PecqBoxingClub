using RamDam.BackEnd.Core.Models.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RamDam.BackEnd.Core.Services
{
   public interface ISchedulingService 
    {
        Task<List<Movie>> GetScheduledMovies();
    }
}
