using RamDam.BackEnd.Core.Models.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RamDam.BackEnd.Core.Repositories
{
    public interface IStoredProcedureRepository
    {
        Task<List<AdminRating>> GetAdminRatings();
        Task<List<UserRating>> GetUserRatings(Guid idUser);
        Task<List<UserRating>> GetUserSubRatings(Guid idUser);
        Task<bool> DeleteUser(Guid id);
        Task<bool> InsertAdminRating(List<GenericAdminRating> subRating);
        Task<List<UserAnswer>> GetUserAnswers();
    }
}
