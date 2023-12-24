using AT_Domain.Models;
using AT_Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace AT_Infrastructure.Repositories
{
    public class UserRepository : IBaseModelRepository<User>
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _context.Users.OrderBy(u => u.CreatedAt)
                .ToListAsync();
        }

        public async Task<User?> GetAsync(Guid id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<User?> GetByNameAsync(string name)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Username == name);
        }

        public async Task<User> AddAsync(User entity)
        {
            await _context.Users.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<User?> UpdateAsync(User entity)
        {
            var existingUser = await GetAsync(entity.Id);
            if (existingUser != null)
            {
                _context.Entry(existingUser).CurrentValues.SetValues(entity);
                await _context.SaveChangesAsync();
                return existingUser;
            }
            return null;
        }

        public async Task DeleteAsync(User entity)
        {
            var user = await _context.Users.FindAsync(entity.Id);
            if (user != null)
            {
                _context.Remove(user);
                await _context.SaveChangesAsync();
            }
        }
    }
}
