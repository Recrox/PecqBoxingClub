using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PecqBoxingClubApi.BackEnd.Core.Models.SP
{
    public class SPIdUser: SPBase
    {
        public Guid _IdUser { get; set; }

        public SPIdUser(string spName, Guid idUser): base(spName)
        {
            _IdUser = idUser;
        }
    }
}
