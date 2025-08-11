using TaskManagementSystem.Models;
using TaskManagementSystem.Repository.IRepository;
using TaskManagementSystem.Service.IService;
using ModelTask = TaskManagementSystem.Models.Task;

namespace TaskManagementSystem.Service
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepository;

        public TaskService(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public async Task<PagedResult<ModelTask>> GetAllTasksAsync(string? status,
            int? assignedToUserId,
            int? teamId,
            DateTime? dueDate,
            int? pageNumber,
            int? pageSize,
            string? sortBy,
            bool sortDesc)
        {
            return await _taskRepository.GetAllAsync(status, assignedToUserId, teamId, dueDate, pageNumber, pageSize, sortBy, sortDesc);
        }

        public async Task<ModelTask?> GetTaskByIdAsync(int id)
        {
            return await _taskRepository.GetByIdAsync(id);
        }

        public async Task<ModelTask> CreateTaskAsync(ModelTask task)
        {
            var created = await _taskRepository.AddAsync(task);
            return created;
        }

        public async System.Threading.Tasks.Task<ModelTask?> UpdateTaskAsync(ModelTask task)
        {
            var updated = await _taskRepository.UpdateAsync(task);
            await _taskRepository.SaveChangesAsync();
            return updated;
        }

        public async Task<bool> DeleteTaskAsync(int id)
        {
            var task = await _taskRepository.GetByIdAsync(id);
            if (task == null)
                return false;

            await _taskRepository.DeleteAsync(task);
            await _taskRepository.SaveChangesAsync();
            return true;
        }
    }
}
