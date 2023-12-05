using RamDam.BackEnd.Core.Models.Api;
using RamDam.BackEnd.Core.Repositories;
using RamDam.BackEnd.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RamDam.BackEnd.Core.Services.Implementations
{
    public class SchedulingService : ISchedulingService
    {
        private readonly ISchedulingRepository _schedulingRepository;

        public SchedulingService(ISchedulingRepository schedulingRepository)
        {
            _schedulingRepository = schedulingRepository;
        }
        public async Task<List<Movie>> GetScheduledMovies()
        {
            //var movies = await _schedulingRepository.GetManyAsync(sch =>
            //    DateTime.Now > sch.StartTime.AddHours(1) 
            //    && DateTime.Now < sch.StartTime.AddHours(sch.Movie.Length.GetTime().Equals(default(DateTime)) ? Hour : 0).AddHours(2));

            var movies = await _schedulingRepository.GetManyAsync();
            var t = movies.Select(movie =>
            {
                var movieLength = movie.Movie.Length.GetTime();
                var endTime = movie.StartTime.AddHours(movieLength.Equals(default(DateTime)) ? 0 : movieLength.Hour)
                                .AddMinutes(movieLength.Equals(default(DateTime)) ? 0 : movieLength.Minute);
                return new { EndTime = endTime, Movie = movie.Movie, StartTime = movie.StartTime };
            });
                return t.Where(mv => DateTime.Now > mv.EndTime && DateTime.Now < mv.EndTime.AddHours(2))
            .Select(mv => mv.Movie).ToList();
            // AddHours(1) 
            //return movies.Select(sch => sch.Movie).ToList();
        }
    }
}
