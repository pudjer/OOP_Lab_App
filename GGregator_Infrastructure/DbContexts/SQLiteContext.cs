using GGregator_Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace GGregator_Infrastructure.DbContexts
{
    public class SQLiteContext : DbContext
    {
        public SQLiteContext(DbContextOptions<SQLiteContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder) { }

        public virtual DbSet<User> Users { get; set; }
    }
}
