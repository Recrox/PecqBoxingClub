
namespace RamDam.BackEnd.Core.Enums
{
    public class Network
    {
        public const string Facebook = "Facebook";
        public const string Google  = "Google";
        public const string Email = "Email";

        public static string[] GetAll()
        {
            return new[]
            {
                Facebook,
                Google,
                Email
            };
        }

    }
}
