using GGregator_Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace GGregator_Infrastructure.DbContexts
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
#if DEBUG
            modelBuilder.Entity<User>()
                .HasData(
                new User
                {
                    Id = Guid.NewGuid(),
                    CreatedAt = DateTime.UtcNow,
                    Username = "admin",
                    Password = BCrypt.Net.BCrypt.HashPassword("password"),
                });
#endif
        }

        public virtual DbSet<User> Users { get; set; }
    }
}
