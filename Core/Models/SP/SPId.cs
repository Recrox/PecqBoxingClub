using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PecqBoxingClubApi.BackEnd.Core.Models.SP
{
    public class SPId: SPBase
    {
        public Guid? _Id { get; set; }
        public bool _Filter { get; set; }

        public SPId(string spName, Guid? id, bool filter) : base(spName)
        {
            _Id = id;
            _Filter = filter;
        }
    }
}
