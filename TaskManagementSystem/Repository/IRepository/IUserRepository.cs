using TaskManagementSystem.Models;

namespace TaskManagementSystem.Repository.IRepository
{
    public interface IUserRepository
    {
        Task<User> GetByEmailAsync(string email);
        Task<User> AddAsync(User user);
        Task<User> GetByIdAsync(int id);
        Task<User> UpdateAsync(User user);
        System.Threading.Tasks.Task DeleteAsync(User user);
        Task<IEnumerable<User>> GetAllAsync();
        System.Threading.Tasks.Task SaveChangesAsync();

    }
}
