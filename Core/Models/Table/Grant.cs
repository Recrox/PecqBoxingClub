using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RamDam.BackEnd.Core.Models.Table
{
    [Table("Grant", Schema = "Dbo")]
    public partial class Grant
    {
        [Key]
        [StringLength(200)]
        public string Key { get; set; }
        [StringLength(50)]
        public string Type { get; set; }
        [StringLength(50)]
        public string SubjectId { get; set; }
        [Required]
        [StringLength(50)]
        public string ClientId { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? ExpirationDate { get; set; }
        [Required]
        public string Data { get; set; }
    }
}
