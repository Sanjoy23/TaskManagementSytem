using TaskManagementSystem.Models;
using TaskManagementSystem.Repository.IRepository;
using TaskManagementSystem.Service.IService;

namespace TaskManagementSystem.Service
{
    public class TeamService : ITeamService
    {
        private readonly ITeamRepository _teamRepository;

        public TeamService(ITeamRepository teamRepository)
        {
            _teamRepository = teamRepository;
        }

        public async Task<IEnumerable<Team>> GetAllTeamsAsync()
        {
            return await _teamRepository.GetAllAsync();
        }

        public async Task<Team?> GetTeamByIdAsync(int id)
        {
            return await _teamRepository.GetByIdAsync(id);
        }

        public async Task<Team?> CreateTeamAsync(Team team)
        {
            var createdTeam = await _teamRepository.AddAsync(team);
            await _teamRepository.SaveChangesAsync();
            return createdTeam;
        }

        public async Task<Team?> UpdateTeamAsync(Team team)
        {
            var updatedTeam = await _teamRepository.UpdateAsync(team);
            await _teamRepository.SaveChangesAsync();
            return updatedTeam;
        }

        public async Task<bool> DeleteTeamAsync(int id)
        {
            var team = await _teamRepository.GetByIdAsync(id);
            if (team == null)
                return false;

            await _teamRepository.DeleteAsync(team);
            await _teamRepository.SaveChangesAsync();
            return true;
        }


    }
}
