using System.ComponentModel.DataAnnotations;

namespace Core.Models.Api;
public class Member : BaseEntity
{
    public string Name { get; set; }
    public string FirstName { get; set; }
    public int Age { get; set; }
    public License? License { get; set; }
    public decimal Solde { get; set; }
}
