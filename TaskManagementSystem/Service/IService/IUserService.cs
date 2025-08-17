using TaskManagementSystem.Models;

namespace TaskManagementSystem.Service.IService
{
    public interface IUserService
    {
        Task<User?> GetUserByEmailAsync(string email);
        Task<User?> GetUserByIdAsync(int id);
        Task<User?> UpdateUserAsync(User user);
        Task<bool> DeleteUserAsync(int id);
        Task<User?> CreateUserAsync(User user);
        Task<IEnumerable<User>> GetAllUsersAsync();
    }
}
