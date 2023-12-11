using Database.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PecqBoxingClubApi.BackEnd.Core.Models.Table;

namespace Database
{
    public class BoxingClubContext : DbContext
    {
        public DbSet<Member> Member { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            IConfiguration configuration = builder.Build();

            // Spécifiez ici votre chaîne de connexion SQL Server
            string connectionString = configuration.GetConnectionString("Database");
            optionsBuilder.UseSqlServer(connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<FinData>().HasKey(f => f.Id);
        }
    }
}





