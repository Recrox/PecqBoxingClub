using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RamDam.BackEnd.Core.Models.Table
{ 
       [Table("Notification", Schema = "dbo")]


     public partial class Notification : ITableObject<Guid>
{

        [Key]
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public DateTime SendDate { get; set; }
        public int IdJob { get; set; }
        public string Topic { get; set; }


    }
}
