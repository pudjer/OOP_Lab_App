using BCrypt.Net;
using GGregator_Domain.Models;
using GGregator_Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GGregator_Tests
{
    internal class AuthTestHelper
    {
        private readonly SQLiteContext _context;
        public AuthTestHelper()
        {
            var contextOptions = new DbContextOptionsBuilder<SQLiteContext>()
                .UseSqlite("Data Source=Local.db").Options;
            _context = new SQLiteContext(contextOptions);
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();
            var testUser = new User
            {
                Username = "test_user",
                Password = BCrypt.Net.BCrypt.HashPassword("password")
            };
        }
    }

}
