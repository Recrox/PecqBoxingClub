using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RamDam.BackEnd.Core.Models.Table
{

    [Table("SocialNetworkUser", Schema = "dbo")]

    public partial class SocialNetworkUser : ITableObject<Guid>
    {

        [Key]
        public Guid Id { get; set; }

        public Guid SocialNetworkId { get; set; }

        public Guid UserId { get; set; }


        [ForeignKey(nameof(SocialNetworkId))]
        public virtual SocialNetwork SocialNetwork { get; set; }
        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; }
       
    }
}
