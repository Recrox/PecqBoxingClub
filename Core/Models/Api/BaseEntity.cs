using System.ComponentModel.DataAnnotations;

namespace Core.Models.Api;
public class BaseEntity
{
    [Key]
    public int Id { get; set; }
}
