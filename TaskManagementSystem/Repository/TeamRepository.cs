using Microsoft.EntityFrameworkCore;
using TaskManagementSystem.Data;
using TaskManagementSystem.Models;
using TaskManagementSystem.Repository.IRepository;

namespace TaskManagementSystem.Repository
{
    public class TeamRepository: ITeamRepository
    {
        private readonly AppDbContext _context;

        public TeamRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Team>> GetAllAsync()
        {
            return await _context.Teams.ToListAsync();
        }

        public async Task<Team?> GetByIdAsync(int id)
        {
            return await _context.Teams.FindAsync(id);
        }

        public async Task<Team> AddAsync(Team team)
        {
            await _context.Teams.AddAsync(team);
            return team;
        }

        public async System.Threading.Tasks.Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<Team> UpdateAsync(Team team)
        {
            _context.Teams.Update(team);
            return team;
        }
        public async System.Threading.Tasks.Task DeleteAsync(Team team)
        {
            _context.Teams.Remove(team);
        }
    }
}
