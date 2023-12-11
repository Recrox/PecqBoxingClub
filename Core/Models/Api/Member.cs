using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models.Api;
public class Member
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string FirstName { get; set; }
    public int Age { get; set; }
    public License License { get; set; }
    public decimal Solde { get; set; }
}
