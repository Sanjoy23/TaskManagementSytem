using Domain.Entities;
using Domain.Interface;
using Infrastruture.Data;
using Microsoft.EntityFrameworkCore;
namespace Infrastruture.Repository
{
    public class TeamRepository: Repository<Team>, ITeamRepository
    {
        private readonly AppDbContext _context;

        public TeamRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Team> GetByNameAsync(string name)
        {
            return await _context.Teams.FirstOrDefaultAsync(x => x.Name == name);
        }
    }
}
