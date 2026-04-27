using Domain.Entities;
using Domain.Interface;
using Infrastruture.Data;
using Microsoft.EntityFrameworkCore;


namespace Infrastruture.Repository
{
    public class UserRepository :  Repository<User>, IUserRepository
    {
        private readonly AppDbContext _context;
        public UserRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            var normalized = email.Trim().ToLowerInvariant();
            var user = await _context.Users
                .Include(x => x.Role)
                .FirstOrDefaultAsync(u => u.Email.ToLower() == normalized);
            if (user == null) {
                return null;
            }
            return user;
        }
    }
}
