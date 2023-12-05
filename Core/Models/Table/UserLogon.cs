using RamDam.BackEnd.Core.Models.Table;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RamDam.BackEnd.Core.Models.Table
{
    [Table("UserLogon", Schema = "RamDam")]
    public partial class UserLogon : ITableObject<Guid>
    {
        [Key]
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime Date { get; set; }
    }
}
