using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RamDam.BackEnd.Core.Models.Table
{
    public partial class Scheduling : ITableObject<Guid>
    {

        [Key]
        public Guid Id { get; set; }

        public Guid? IdMovie { get; set; }

        public DateTime StartTime { get; set; }
        public long? IdJob { get; set; }

        [ForeignKey(nameof(IdMovie))]
        public virtual Movie Movie { get; set; }
        public bool TeamAttendance { get; set; }

    }
}
