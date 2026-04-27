using Domain.Entities;

namespace Domain.Interface
{
    public interface ITeamRepository : IRepository<Team>
    {
        Task<Team> GetByNameAsync(string name);
    }
}
