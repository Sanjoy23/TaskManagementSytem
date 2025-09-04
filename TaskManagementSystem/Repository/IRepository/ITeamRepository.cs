using TaskManagementSystem.Models;

namespace TaskManagementSystem.Repository.IRepository
{
    public interface ITeamRepository : IRepository<Team>
    {
        Task<Team> GetByNameAsync(string name);
    }
}
