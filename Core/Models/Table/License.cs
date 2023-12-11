using Database.Models;

namespace PecqBoxingClubApi.BackEnd.Core.Models.Table
{
    public class License : BaseEntity
    {
        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
