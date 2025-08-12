using TaskManagementSystem.Models;
using TaskManagementSystem.Models.DTOs;
namespace TaskManagementSystem.Service.IService
{
    public interface ITaskService
    {
        Task<PagedResultDto<TaskDto>> GetAllTasksAsync(string? status,
     int? assignedToUserId,
     int? teamId,
     DateTime? dueDate,
     int? pageNumber,
     int? pageSize,
     string? sortBy,
     bool sortDesc);
        Task<TaskDto?> GetTaskByIdAsync(int id);
        Task<TaskEntity> GetTaskByTitleAsync(string name);
        Task<TaskEntity> CreateTaskAsync(TaskEntity task);
        Task<TaskDto?> UpdateTaskAsync(TaskEntity task);
        Task<bool> DeleteTaskAsync(int id);
    }
}
