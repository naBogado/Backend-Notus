using Notus.Models.Role;
using Notus.Models.User;
using Microsoft.EntityFrameworkCore;
using Notus.Models.Class;
using Microsoft.Extensions.Logging;
using Notus.Models.Event;

namespace Notus.Config
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Class> Clases { get; set; }
        public DbSet<Event> Events { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasIndex(x => x.Email).IsUnique();
            modelBuilder.Entity<Role>().HasIndex(x => x.Name).IsUnique();

            modelBuilder.Entity<User>()
                .HasMany(x => x.Roles)
                .WithMany()
                .UsingEntity<UserRoles>(
                    l => l.HasOne<Role>().WithMany().HasForeignKey(x => x.RoleId),
                    r => r.HasOne<User>().WithMany().HasForeignKey(x => x.UserId)
                );
        }
    }   
}
