using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RamDam.BackEnd.Core.Models.Table
{ 
     [Table("Rating", Schema = "dbo")]


    public partial class Rating : ITableObject<Guid>
    {


        [Key]
        public Guid Id { get; set; }

        public Guid? IdUser { get; set; }

        public Guid IdMovie { get; set; }

        public int? Note { get; set; }

        public int? DisturbingRate { get; set; }

        public int? Answer { get; set; }

        [ForeignKey(nameof(IdUser))]
        public virtual User User { get; set; }
        [ForeignKey(nameof(IdMovie))]
        public virtual Movie Movie { get; set; }
    }
}

