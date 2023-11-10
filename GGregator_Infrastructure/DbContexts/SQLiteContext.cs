using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GGregator_Infrastructure.DbContexts
{
    public class SQLiteContext : DbContext
    {
        public SQLiteContext(DbContextOptions<SQLiteContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder) { }
    }
}
