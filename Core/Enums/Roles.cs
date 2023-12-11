namespace PecqBoxingClubApi.BackEnd.Core.Enums
{
    public class Roles
    {
        public const string SuperAdmin = "SuperAdmin";
        public const string Admin = "Admin";
        public const string User = "User";

        public static string Get(string application, string role) => $"{application}${role}";
    }
}
