using BCrypt.Net;
using GGregator_Domain.DTOs;
using GGregator_Domain.Models;
using GGregator_Infrastructure.DbContexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace GGregator_Infrastructure.Facades
{
    public class AppAuthFacade : IAuthenticationFacade
    {
        private readonly SQLiteContext _context;
        private readonly IConfiguration _configuration;
        public AppAuthFacade(SQLiteContext context, IConfiguration configuration)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }
        public async Task<SignedUpDTO?> Register(string username, string password)
        {
            // validation for whether the username already exists will be later
            // maybe...
            throw new NotImplementedException();
        }

        public async Task<LoggedInDTO?> Authenticate(string username, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
            if (user == null)
            {
                return null;
            }

            if (BCrypt.Net.BCrypt.Verify(password, user.Password))
            {
                return new LoggedInDTO
                {
                    Token = GenerateToken(username)
                };
            }

            return null;
        }

        private string GenerateToken(string username)
        {
            //var sampleToken = "eyJhbGciOiJIUzI1NiJ9.eyJSb2xlIjoiQWRtaW4iLCJJc3N1ZXIiOiJHR3JlZ2F0b3IuQXV0aCIsIlVzZXJuYW1lIjoiVGVzdFVzZXIiLCJBdWRpZW5jZSI6IkdHcmVnYXRvci5BdXRoLkNsaWVudCIsImV4cCI6MTcwMjQxMTEyOSwiaWF0IjoxNjk5ODE5MTI5fQ.VVLmJ9wxi2hHDwBgOGF9GE6LCWDrPRKqj2EJaOACrcM";

            var keyString = _configuration.GetSection("JWT Bearer")
                .GetValue<string>("SymmetricSecurityKey");

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyString));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var issuer = _configuration.GetSection("JWT Bearer")
                .GetValue<string>("Issuer");
            var audience = _configuration.GetSection("JWT Bearer")
                .GetValue<string>("Audience");

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, username),
            };

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                // would also probably have to wrestle with the timezone dark gods later
                expires: TokenExpirationDate(DateTime.Now),
                signingCredentials: credentials);

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
            return tokenString;
        }

        private DateTime TokenExpirationDate(DateTime timestamp )
        {
            return timestamp.AddDays(1997);
        }
    }
}
