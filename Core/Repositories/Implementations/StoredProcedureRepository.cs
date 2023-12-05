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

        public StoredProcedureRepository(RamDamContext context,
            GlobalSettings globalSettings
            ): base(context)
        {
            _globalSettings=globalSettings;
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
        public async Task<List<UserAnswer>> GetUserAnswers()
        {
            var spBase = new SPBase("[dbo].[usp_GetUserAnswers]");
            return await ExecuteQueryStoredProcedure<UserAnswer>(spBase);
        }
    }
}
