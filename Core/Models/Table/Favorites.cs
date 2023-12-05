using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RamDam.BackEnd.Core.Models.Table
{

    [Table("Favorites", Schema = "dbo")]
    public partial class Favorites : ITableObject<Guid>
    {

        [Key]
        public Guid Id { get; set; }

        public Guid? IdUser { get; set; }

        public Guid? IdMovie { get; set; }

        public DateTime? StartTime { get; set; }

        public DateTime? EndTime { get; set; }
        
        public Guid? IdScheduling { get; set; }


        [ForeignKey(nameof(IdUser))]
        public virtual User User { get; set; }
        [ForeignKey(nameof(IdMovie))]
        public virtual Movie Movie { get; set; }
        [ForeignKey(nameof(IdScheduling))]
        public virtual Scheduling Scheduling { get; set; }

    }
}
