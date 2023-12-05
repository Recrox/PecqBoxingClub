using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RamDam.BackEnd.Core.Models.Table
{
    [Table("Role", Schema = "dbo")]
    public class Role : ITableObject<Guid>
    {
        [Key]
        public Guid Id { get; set; }
        public string RoleName { get; set; }
    }
}
