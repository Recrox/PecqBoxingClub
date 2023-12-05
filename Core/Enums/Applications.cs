namespace RamDam.BackEnd.Core.Enums
{
    public class Applications
    {
        public const string Administration = "Administration";

        public static string[] GetAll()
        {
            return new[]
            {
                Administration
            };
        }
    }
}
