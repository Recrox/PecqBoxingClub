using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RamDam.BackEnd.Core.Models.Api
{
    public class GenericAdminRating
    {
        public Guid Id { get; set; }
        public int Rate { get; set; }
        public int DisturbingRate { get; set; }
    }
}
