using AT_Domain.DTOs.OutDTOs;
using AT_Domain.Models;
using AT_Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AT_Infrastructure.Facades
{
    // если использовать паттерн Aggregate, то, видимо, только одному классу нужно
    // будет работать как с юзерами, так и подписками? (т. е. этот фасад объединить с
    // SubscriptionFacade) ну или как-то так хз крч
    public class AppAuthFacade : IAuthenticationFacade
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;
        public AppAuthFacade(AppDbContext context, IConfiguration configuration)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }
        public async Task<SignedUpDTO?> Register(string username, string password)
        {
            var existUser = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
            if (existUser != null)
            {
                return null;
            }
            var newUser = new User
            {
                Username = username,
                CreatedAt = DateTime.UtcNow,
                Password = BCrypt.Net.BCrypt.HashPassword(password),
                IsAdmin = false,
                IsSubscribed = false,
            };

            await _context.AddAsync(newUser);
            await _context.SaveChangesAsync();
            
            var userToken = GenerateToken(newUser.Username,
                newUser.Id.ToString(), 
                newUser.IsAdmin,
                newUser.IsSubscribed);

            return new SignedUpDTO
            {
                Id = newUser.Id,
                Username = newUser.Username,
                Token = userToken
            };
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
                    Token = GenerateToken(username, user.Id.ToString(),
                    user.IsAdmin, user.IsSubscribed)
                };
            }

            return null;
        }

        private string GenerateToken(string username, string id, bool isAdmin, bool isSubscribed)
        {
            var keyString = _configuration["Bearer:Key"];

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyString));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var issuer = _configuration["Bearer:Issuer"];
            var audience = _configuration["Bearer:Audience"];

            var claims = new List<Claim>
            {
                new Claim("id", id),
                new Claim("name", username),
                new Claim("is_admin", isAdmin.ToString(), ClaimValueTypes.Boolean),
                new Claim("is_subscribed", isSubscribed.ToString(), ClaimValueTypes.Boolean),
            };

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: TokenExpirationDate(DateTime.UtcNow),
                signingCredentials: credentials);

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
            return tokenString;
        }

        private DateTime TokenExpirationDate(DateTime timestamp)
        {
            return timestamp.Add(TimeSpan.FromDays(99));
        }
    }
}
