using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RamDam.BackEnd.Core.Models.Api
{
    public class ScheduledMovie
    {
        public IEnumerable<Guid> ConflictedSchedulings { get; set; }
        public Favorites Scheduling { get; set; }
    }
}
