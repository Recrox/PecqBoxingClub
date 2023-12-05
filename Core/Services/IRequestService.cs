using RamDam.BackEnd.Core.Models.RamDamApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RamDam.BackEnd.Core.Services
{
    public interface IRequestService
    {
        Task<List<RamDamPlace>> GetRamDamPlaces(bool isHotel);
        Task<List<RamDamPartner>> GetRamDamPartner();
        Task<List<RamDamGuest>> GetRamDamGuest();
        Task<List<RamDamMovie>> GetRamDamMovies();
    }
    
}
