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
        public SQLiteContext Context { get; set; }
        public AuthTestHelper()
        {
            var contextOptions = new DbContextOptionsBuilder<SQLiteContext>()
                // TODO: decouple this stuff too
                .UseSqlite("Data Source=Local.db").Options;
            Context = new SQLiteContext(contextOptions);
            Context.Database.EnsureDeleted();
            Context.Database.EnsureCreated();
            var testUser = new User
            {
                Username = "test_user",
                Password = BCrypt.Net.BCrypt.HashPassword("password")
            };
            Context.Users.Add(testUser);
            Context.SaveChanges();
            Context.ChangeTracker.Clear();
        }
    }

}
