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

        public Task<Team> GetById(int id)
        {
            return _teamRepository.GetById(id);
        }

        public Task<IEnumerable<Team>> GetAll()
        {
            return _teamRepository.GetAll();
        }

        public void Add(Team entity)
        {
            _teamRepository.Add(entity);
        }

        public void Update(Team entity)
        {
            _teamRepository.Update(entity);
        }

        public void Delete(Team entity)
        {
            _teamRepository.Delete(entity);
        }
    }
}
