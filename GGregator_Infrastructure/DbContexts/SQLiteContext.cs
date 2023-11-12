using GGregator_Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace GGregator_Infrastructure.DbContexts
{
    public class SQLiteContext : DbContext
    {
        public SQLiteContext(DbContextOptions<SQLiteContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
#if DEBUG
            modelBuilder.Entity<User>()
                .HasData(
                new User
                {
                    Id = Guid.NewGuid(),
                    CreatedAt = DateTime.Now,
                    Username = "admin",
                    Password = BCrypt.Net.BCrypt.HashPassword("password"),
                });
#endif
        }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Subscription> Subscriptions { get; set; }
    }
}
