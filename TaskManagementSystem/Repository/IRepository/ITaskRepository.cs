using TaskManagementSystem.Models;
namespace TaskManagementSystem.Repository.IRepository
{
    public interface ITaskRepository : IRepository<TaskEntity>
    {
        Task<TaskEntity?> GetByIdAsync(string id);
        Task<TaskEntity?> GetByTitleAsync(string title);
    }
}
