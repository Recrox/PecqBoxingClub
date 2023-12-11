using System.ComponentModel.DataAnnotations;

namespace Database.Models
{
    public class BaseEntity
    {
        [Key]
        public int Id { get; set; }
    }
}
