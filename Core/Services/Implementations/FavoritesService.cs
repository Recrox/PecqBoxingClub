using RamDam.BackEnd.Core.Exceptions;
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
    public class FavoritesService : IFavoritesService
    {
        private readonly IFavoritesRepository _favoritesRepository;
        private readonly ISchedulingRepository _schedulingRepository;

        public FavoritesService(IFavoritesRepository favoritesRepository, ISchedulingRepository schedulingRepository)
        {
            _favoritesRepository = favoritesRepository;
            _schedulingRepository = schedulingRepository;
        }

        public async Task<ScheduledMovie> AddFavorite(Favorites favoriteRequest)
        {
            var favorites = await _favoritesRepository.GetManyAsync(favorite => favorite.IdUser.Equals(favoriteRequest.User.Id));
            var scheduling = await _schedulingRepository.GetByIdAsync(favoriteRequest.Scheduling.Id);
            var alreadyTaken = favorites.Where(fav => fav.Scheduling.Id.Equals(favoriteRequest.Scheduling.Id));
            if (alreadyTaken.Any())
                return null;
            var fav = await _favoritesRepository.UpsertAsync(favoriteRequest);
            IEnumerable<Favorites> timeConflictedFavorites = this.HasTimeConflict(favorites, scheduling);
            return new ScheduledMovie()
            {
                ConflictedSchedulings = timeConflictedFavorites.Any() ? timeConflictedFavorites.Select(fav => fav.Scheduling.Id) : null,
                Scheduling = fav
            };
            //if (!timeConflictedFavorites.Any())
            //{
            //    return await _favoritesRepository.UpsertAsync(favoriteRequest);
            //}
            //throw new TimeConflictException(timeConflictedFavorites);
        }

        public async Task<List<GroupedPersonalGridItem>> GetSchedulings(Guid userId)
        {
            var favorites = await _favoritesRepository.GetManyAsync(favorite => favorite.IdUser.Equals(userId));
            return favorites.GroupBy(fav => fav.Scheduling.StartTime.Date)
                .Select(gr => new GroupedPersonalGridItem() 
                {
                    Date = gr.Key,
                    Items = gr.Select(favorite => new PersonalGridItem()
                    {
                        Attach = favorite.Scheduling.Movie.Attach,
                        IdFavorite = favorite.Id,
                        MovieTitle = favorite.Scheduling.Movie.Title,
                        IdMovie = favorite.Scheduling.Movie.Id,
                        StartTime = favorite.Scheduling.StartTime,
                        IdScheduling = favorite.Scheduling.Id,
                        ConflictedSchedulings = this.HasTimeConflict(favorites, favorite.Scheduling).Select(f => f.Scheduling.Id).ToListOrNull()
                    }).ToList()
                }).OrderBy(fa => fa.Date).ToList();
        }

        private IEnumerable<Favorites> HasTimeConflict(IEnumerable<Favorites> favorites, Scheduling newScheduling)
        {
            var newMovieLength = newScheduling.Movie.Length.GetTime();
            var newMovieEndTime = newScheduling.StartTime.AddHours(newMovieLength.Equals(default(DateTime)) ? 0 : newMovieLength.Hour)
                                .AddMinutes(newMovieLength.Equals(default(DateTime)) ? 0 : newMovieLength.Minute);

            var conflictedFavorites = favorites
                                            .Where(fav => fav.Scheduling.StartTime.Date.Equals(newScheduling.StartTime.Date) && !fav.Scheduling.Id.Equals(newScheduling.Id))//only those of the same day and different id
                                            .Where(fav =>
                                            {
                                                var movieLength = fav.Scheduling.Movie.Length.GetTime();
                                                var endTime = fav.Scheduling.StartTime.AddHours(movieLength.Equals(default(DateTime)) ? 0 : movieLength.Hour)
                                                                .AddMinutes(movieLength.Equals(default(DateTime)) ? 0 : movieLength.Minute);
                                                return (newScheduling.StartTime <= endTime && fav.Scheduling.StartTime <= newMovieEndTime );
                                            });
            return conflictedFavorites;
        }
    }
}
