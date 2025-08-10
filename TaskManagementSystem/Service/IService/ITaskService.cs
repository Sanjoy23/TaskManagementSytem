using System.Threading.Tasks;
using TaskManagementSystem.Models;
using ModelTask = TaskManagementSystem.Models.Task;
namespace TaskManagementSystem.Service.IService
{
    public interface ITaskService
    {
        Task<PagedResult<ModelTask>> GetAllTasksAsync(string? status,
    int? assignedToUserId,
    int? teamId,
    DateTime? dueDate,
    int? pageNumber,
    int? pageSize,
    string? sortBy,
    bool sortDesc);
        Task<ModelTask?> GetTaskByIdAsync(int id);
        Task<ModelTask> CreateTaskAsync(ModelTask task);
        Task<ModelTask?> UpdateTaskAsync(ModelTask task);
        Task<bool> DeleteTaskAsync(int id);
    }
}
