using TaskManagementSystem.Models;

namespace TaskManagementSystem.Repository.IRepository
{
    public interface ITeamRepository
    {
        Task<IEnumerable<Team>> GetAllAsync();
        Task<Team?> GetByIdAsync(int id);
        Task<Team> AddAsync(Team team);
        Task<Team> UpdateAsync(Team team);
        System.Threading.Tasks.Task DeleteAsync(Team team);
        System.Threading.Tasks.Task SaveChangesAsync();
    }
}
