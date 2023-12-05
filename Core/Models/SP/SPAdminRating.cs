using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RamDam.BackEnd.Core.Models.SP
{
    public class SPAdminRating: SPBase
    {
        public Guid? _IdUser { get; set; }
        public Guid _Id { get; set; }
        public int _Rate { get; set; }
        public int _DisturbingRate { get; set; }

        public SPAdminRating(string spName, Guid id, int rate, int disturbingRate, Guid? idUser = null) : base(spName)
        {
            _Id = id;
            _Rate = rate;
            _DisturbingRate = disturbingRate;
            _IdUser=idUser;
        }
    }
}
