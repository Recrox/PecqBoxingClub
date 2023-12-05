using RamDam.BackEnd.Core.Models.Api;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RamDam.BackEnd.Core.Models.Table
{

    [Table("Movie", Schema = "dbo")]

    public partial class Movie : ITableObject<Guid>
    {

        public Movie()
        {
            Scheduling = new HashSet<Scheduling>();
        }
        [Key]
        public Guid Id { get; set; }

        public string Title { get; set; }

        [StringLength(50)]
        public string Realisator { get; set; }
        [StringLength(50)]
        public string Length { get; set; }

        public string Description { get; set; }

        public string Attach { get; set; }

        public string Link { get; set; }
        public string Trailer { get; set; }
        public long? IdRamDamMovie { get; set; }
        public bool CanVote { get; set; }

        [InverseProperty (nameof(Table.Scheduling.Movie))] 
        public virtual ICollection<Scheduling> Scheduling { get; set; }

        public virtual ICollection<SubMovie> Submovies { get; set; }
    }
}