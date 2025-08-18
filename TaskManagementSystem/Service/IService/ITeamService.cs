using TaskManagementSystem.Models;

namespace TaskManagementSystem.Service.IService
{
    public interface ITeamService
    {
        Task<Team> GetById(int id);
        Task<IEnumerable<Team>> GetAll();
        void Add(Team entity);
        void Update(Team entity);
        void Delete(Team entity);
    }
}
