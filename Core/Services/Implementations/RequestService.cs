using RamDam.BackEnd.Configuration;
using RamDam.BackEnd.Core.Models.RamDamApi;
using RamDam.BackEnd.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RamDam.BackEnd.Core.Services.Implementations
{
    public class RequestService: RequestBaseService, IRequestService
    {
        public RequestService(GlobalSettings globalSettings): base(globalSettings)
        {
        }

        public async Task<List<RamDamPlace>> GetRamDamPlaces(bool isHotel)
        {
            var list = await SendAsync<List<RamDamPlace>>(System.Net.Http.HttpMethod.Get, "address?per_page=100");
            list.ForEach(pl => pl.Acf.Place = pl.Acf.Place.GetType() == typeof(bool) ? new Place() : ((Newtonsoft.Json.Linq.JObject)pl.Acf.Place).ToObject<Place>());
            var hotel = list.Where(place => isHotel ? place.Acf.Addresstype.Equals("hotel") : place.Acf.Addresstype.Equals("restaurant"))
                .Where(status => status.Status.Equals("publish")).ToList();
           
            return hotel;
           
        }
        public async Task<List<RamDamPartner>> GetRamDamPartner()
        {
            return await SendAsync<List<RamDamPartner>>(System.Net.Http.HttpMethod.Get, "partner?per_page=100");
        }
        public async Task<List<RamDamGuest>> GetRamDamGuest()
        {
            return await SendAsync<List<RamDamGuest>>(System.Net.Http.HttpMethod.Get, "guest?per_page=100");
        }

        public async Task<List<RamDamMovie>> GetRamDamMovies()
        {
            return await SendAsync<List<RamDamMovie>>(System.Net.Http.HttpMethod.Get, "movie?per_page=100");
        }
    }
}
