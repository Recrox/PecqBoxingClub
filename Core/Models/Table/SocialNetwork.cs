using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RamDam.BackEnd.Core.Models.Table
{

    [Table("SocialNetwork", Schema = "dbo")]

    public partial class SocialNetwork : ITableObject<Guid>
    {

        [Key]
        public Guid Id { get; set; }

       
        [StringLength(50)]
        public String Social { get; set; }


        public virtual ICollection<User> User { get; set; }


        //[ForeignKey(nameof(IdMovie))]
        //public virtual Movie Movie { get; set; }

    }
}
