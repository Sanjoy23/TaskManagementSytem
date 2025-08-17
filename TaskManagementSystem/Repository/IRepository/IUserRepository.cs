using TaskManagementSystem.Models;

namespace TaskManagementSystem.Repository.IRepository
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User> GetByEmailAsync(string email);
    }
}
