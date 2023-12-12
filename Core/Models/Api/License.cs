using System.ComponentModel.DataAnnotations;

namespace Core.Models.Api;


public class License : BaseEntity
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}
