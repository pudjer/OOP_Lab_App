using GGregator_Domain.DTOs;
using GGregator_Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GGregator_Infrastructure.Facades
{
    public interface IAuthenticationFacade
    {
        public Task<string?> Authenticate(string username, string password);

        public Task<UserDTO?> Register(string username, string password);
    }
}
