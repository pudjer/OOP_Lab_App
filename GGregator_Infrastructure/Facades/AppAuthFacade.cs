using GGregator_Domain.DTOs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GGregator_Infrastructure.Facades
{
    public class AppAuthFacade : IAuthenticationFacade
    {
        private readonly DbContext _context;
        public AppAuthFacade(DbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public async Task<SignedUpDTO?> Register(string username, string password)
        {
            throw new NotImplementedException();
        }

        public async Task<LoggedInDTO?> Authenticate(string username, string password)
        {
            throw new NotImplementedException();
        }
    }
}
