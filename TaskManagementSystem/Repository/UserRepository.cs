using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TaskManagementSystem.Data;
using TaskManagementSystem.Models;
using TaskManagementSystem.Repository.IRepository;

namespace TaskManagementSystem.Repository
{
    public class UserRepository :  IUserRepository
    {
        private readonly AppDbContext _context;
        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<User> AddAsync(User user)
        {
           await _context.Users.AddAsync(user);
           await _context.SaveChangesAsync();
           return user;
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            var user = await _context.Users
                         .FirstOrDefaultAsync(u => u.Email == email);
            if (user == null) {
                return null;
            }
            return user;
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async System.Threading.Tasks.Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<User> UpdateAsync(User user)
        {
            _context.Users.Update(user);
            return user;
        }

        public async System.Threading.Tasks.Task DeleteAsync(User user)
        {
            _context.Users.Remove(user);
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _context.Users.ToListAsync();
        }
    }
}
