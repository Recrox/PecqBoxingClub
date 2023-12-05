using RamDam.BackEnd.Core.Models.Table;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using RamDam.BackEnd.Core.Enums;
using RamDam.BackEnd.Core.Models.Table;

namespace RamDam.BackEnd.Core
{
    public partial class RamDamContext : DbContext
    {
        public RamDamContext()
        {
        }

        public RamDamContext(DbContextOptions<RamDamContext> options)
            : base(options)
        {
        }

        // Hub
        public virtual DbSet<Grant> Grants { get; set; }
        public virtual DbSet<Movie> Movie { get; set; }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<RoleUser> RoleUsers { get; set; }
        public virtual DbSet<Rating> Rating { get; set; }
        public virtual DbSet<Scheduling> Scheduling { get; set; }
        public virtual DbSet<Favorites> Favorites { get; set; } 
        public virtual DbSet<Notification> Notifications { get; set; }
        //public virtual DbSet<UserLogon> UserLogons { get; set; }
     

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning // To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=vbizzsqldev01;Database=FTSO;Trusted_Connection=True;");
                
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Grant>(entity =>
            {
                entity.HasKey(e => e.Key)
                    .HasName("PK_tb_Grant");
            });

            

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.Email).IsUnicode(false);

     
                entity.Property(e => e.Password).IsUnicode(false);

                entity.Property(e => e.UserName).IsUnicode(false);
            });
          
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
