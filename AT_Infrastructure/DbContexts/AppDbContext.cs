using AT_Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace AT_Infrastructure.DbContexts
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
                    CreatedAt = new DateTime(2023, 9, 17, 19, 19, 19, 97, DateTimeKind.Utc),
                    Username = "admin",
                    Password = BCrypt.Net.BCrypt.HashPassword("password"),
                    IsAdmin = true,
                    IsSubscribed = true,
                });
#endif
        }

        public virtual DbSet<User> Users { get; set; }
    }
}
