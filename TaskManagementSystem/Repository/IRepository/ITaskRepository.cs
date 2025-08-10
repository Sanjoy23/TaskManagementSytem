using TaskManagementSystem.Models;
using ModelTask = TaskManagementSystem.Models.Task;
namespace TaskManagementSystem.Repository.IRepository
{
    public interface ITaskRepository
    {
        Task<PagedResult<ModelTask>> GetAllAsync(string? status,
    int? assignedToUserId,
    int? teamId,
    DateTime? dueDate,
    int? pageNumber,
    int? pageSize,
    string? sortBy,
    bool sortDesc);
        Task<ModelTask?> GetByIdAsync(int id);
        Task<ModelTask> AddAsync(ModelTask task);
        Task<ModelTask> UpdateAsync(ModelTask task);
        System.Threading.Tasks.Task DeleteAsync(ModelTask task);
        System.Threading.Tasks.Task SaveChangesAsync();
    }
}
