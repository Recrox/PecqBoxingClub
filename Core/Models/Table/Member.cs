using Database.Models;

namespace PecqBoxingClubApi.BackEnd.Core.Models.Table
{
    public class Member : BaseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string FirstName { get; set; }
        public int Age { get; set; }
        public License License { get; set; }
        public decimal Solde { get; set; }
    }
}
