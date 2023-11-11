using GGregator_Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GGregator_Infrastructure.Facades
{
    public class AppAuthFacade : IAuthenticationFacade
    {
        public async Task<string?> Authenticate(string username, string password)
        {
            throw new NotImplementedException();
        }

        public async Task<UserDTO?> Register(string username, string password)
        {
            throw new NotImplementedException();
        }
    }
}
