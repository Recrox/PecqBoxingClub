using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RamDam.BackEnd.Core.Models.Api
{
    public class PersonalGridItem
    {
        public Guid IdFavorite { get; set; }
        public string MovieTitle { get; set; }
        public string Attach { get; set; }
        public DateTime StartTime { get; set; }
        public Guid IdScheduling { get; set; }

        public Guid IdMovie { get; set; }
        public IEnumerable<Guid> ConflictedSchedulings { get; set; }
    }
}
