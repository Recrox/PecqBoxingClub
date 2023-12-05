using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RamDam.BackEnd.Core.Models.Table
{
    [Table("RoleUser", Schema = "dbo")]
    public class RoleUser : ITableObject<Guid>
    {
        public Guid Id { get; set; }
        public Guid IdUser { get; set; }
        public Guid IdRole { get; set; }

        [ForeignKey(nameof(IdUser))]
        public virtual User User { get; set; }
        [ForeignKey(nameof(IdRole))]
        public virtual Role Role { get; set; }
    }
}
