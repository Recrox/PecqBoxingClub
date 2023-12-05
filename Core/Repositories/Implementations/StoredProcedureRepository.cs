using Newtonsoft.Json;
using RamDam.BackEnd.Configuration;
using RamDam.BackEnd.Core.Models.Api;
using RamDam.BackEnd.Core.Models.SP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RamDam.BackEnd.Core.Repositories.Implementations
{
    public class StoredProcedureRepository : SPBaseRepository, IStoredProcedureRepository
    {
        private readonly GlobalSettings _globalSettings;
        private readonly IMovieRepository _movieRepository;

        public StoredProcedureRepository(RamDamContext context,
            GlobalSettings globalSettings,
            IMovieRepository movieRepository): base(context)
        {
            _globalSettings=globalSettings;
            _movieRepository=movieRepository;
        }
        public async Task<List<AdminRating>> GetAdminRatings()
        {
            var spBase = new SPBase("dbo.usp_GetRatings");
            List<string> stringResult = await ExecuteQueryStoredProcedure<string>(spBase);
            return JsonConvert.DeserializeObject<List<AdminRating>>(string.Join(string.Empty, stringResult));
        }

        public async Task<List<UserRating>> GetUserRatings(Guid idUser)
        {
            var spIdUser = new SPIdUser("dbo.usp_GetuserRatings", idUser);
            return await ExecuteQueryStoredProcedure<UserRating>(spIdUser);
        }

        public async Task<List<UserRating>> GetUserSubRatings(Guid idUser)
        {
            var spIdUser = new SPIdUser("dbo.usp_GetUserSubRatings", idUser);
            return await ExecuteQueryStoredProcedure<UserRating>(spIdUser);
        }

        public async Task<bool> DeleteUser(Guid id)
        {
            var spIdUser = new SPIdUser("dbo.usp_DeleteUser", id);
            return await ExecuteStoredProcedure(spIdUser);
        }

        public async Task<bool> InsertAdminRating(List<GenericAdminRating> subRating)
        {
            foreach (var rating in subRating)
            {
                var spSubRating = new SPAdminRating("dbo.usp_InsertRating", rating.Id, rating.Rate, rating.DisturbingRate);
                if (!await ExecuteStoredProcedure(spSubRating))
                    return false;
            }
            return true;
        }

        public async Task<bool> InsertUserRating(GenericUserRating subRating)
        {
            var parentMovie = await _movieRepository.GetFirstAsync(movie => movie.Id == subRating.IdParentMovie);

            if (parentMovie == null
                || string.IsNullOrEmpty(parentMovie.Length)
                || !CanVote(parentMovie))
            {
                return false;
            }

            var spSubRating = new SPAdminRating("dbo.usp_InsertRating",
                                            subRating.IdSubMovie,
                                            subRating.Rate,
                                            subRating.DisturbingRate,
                                            subRating.User?.Id ?? null
                                            );

            return await ExecuteStoredProcedure(spSubRating);
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

        public async Task<List<MovieAdmin>> GetMoviesAdmin(Guid? id, bool filter)
        {
            var spBase = new SPId("dbo.usp_GetMoviesAdmin", id, filter);
            List<string> stringResult = await ExecuteQueryStoredProcedure<string>(spBase);
            return JsonConvert.DeserializeObject<List<MovieAdmin>>(string.Join(string.Empty, stringResult));
        }

        public async Task<List<UserAnswer>> GetUserAnswers()
        {
            var spBase = new SPBase("[dbo].[usp_GetUserAnswers]");
            return await ExecuteQueryStoredProcedure<UserAnswer>(spBase);
        }

        
    }
}
