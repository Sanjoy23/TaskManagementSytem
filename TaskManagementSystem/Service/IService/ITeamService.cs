using TaskManagementSystem.Models;

namespace TaskManagementSystem.Service.IService
{
    public interface ITeamService
    {
        Task<IEnumerable<Team>> GetAllTeamsAsync();
        Task<Team?> GetTeamByIdAsync(int id);
        Task<Team?> UpdateTeamAsync(Team team);
        Task<Team?> CreateTeamAsync(Team team);
        Task<bool> DeleteTeamAsync(int id);
    }
}
