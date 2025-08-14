using TaskManagementSystem.Models;

namespace TaskManagementSystem.Service.IService
{
    public interface IUserService
    {
        Task<User> GetById(int id);
        Task<IEnumerable<User>> GetAll();
        void Add(User entity);
        void Update(User entity);
        void Delete(User entity);
        Task<User?> GetUserByEmailAsync(string email);
        
    }
}
