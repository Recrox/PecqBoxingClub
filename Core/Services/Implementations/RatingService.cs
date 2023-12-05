using Microsoft.EntityFrameworkCore.Metadata.Internal;
using RamDam.BackEnd.Configuration;
using RamDam.BackEnd.Core.Models.Api;
using RamDam.BackEnd.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RamDam.BackEnd.Core.Services.Implementations
{
    public class RatingService : IRatingService
    {
        private readonly IRatingRepository _ratingRepository;
        private readonly IMovieRepository _movieRepository;
        private readonly GlobalSettings _globalSettings;

        public RatingService(IRatingRepository ratingRepository, IMovieRepository movieRepository, GlobalSettings globalSettings)
        {
            _ratingRepository = ratingRepository;
            _movieRepository = movieRepository;
            _globalSettings = globalSettings;
        } 

        public async Task<Rating> UpsertRating(Rating rating)
        {
            var previousRating = await _ratingRepository.GetFirstAsync(r => r.IdUser.Equals(rating.User.Id) && r.IdMovie.Equals(rating.Movie.Id));
            if (previousRating != null)
                rating.Id = previousRating.Id;
            var movie = await _movieRepository.GetFirstAsync(movie => movie.Id == rating.Movie.Id);
            if (!string.IsNullOrEmpty(movie.Length))
            {
                if (CanVote(movie))
                {
                    return await _ratingRepository.UpsertAsync(rating);
                }
            }
            return null;
        }

        private bool CanVote(Movie movie)
        {
            var movieLengthSplit = movie.Length.ToLower().Split('h');
            return movie.Scheduling
                                .Where(sch => sch.StartTime <= DateTime.Now
                                && (sch.StartTime.AddHours(int.Parse(movieLengthSplit[0]))
                                                 .AddMinutes(int.Parse(movieLengthSplit[1]) + _globalSettings.MinutesForVoting)
                                                 >= DateTime.Now))
                                                 .Any();
        }
    }
}
