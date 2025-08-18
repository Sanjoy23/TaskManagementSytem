using TaskManagementSystem.Models;
using TaskManagementSystem.Repository.IRepository;
using TaskManagementSystem.Service.IService;

namespace TaskManagementSystem.Service
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public void Add(User user)
        {
            _userRepository.Add(user);
        }
        public void Update(User user) { 
            _userRepository.Update(user);
        }

        public async Task<User> GetById(int id)
        {
            return await _userRepository.GetById(id);
        }
        public void Delete(User user)
        {
            _userRepository.Delete(user);
        }

        public Task<IEnumerable<User>> GetAll()
        {
            return _userRepository.GetAll();
        }
        public async Task<User?> GetUserByEmailAsync(string email)
        {
            var user = await _userRepository.GetByEmailAsync(email);
            if (user == null)
            {
                return null;
            }
            return user;
        }
    }
}
