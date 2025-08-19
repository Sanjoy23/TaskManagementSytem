using System.Linq.Expressions;
using TaskManagementSystem.Models;
using TaskManagementSystem.Models.DTOs;
using TaskManagementSystem.Models.ResponseDtos;
namespace TaskManagementSystem.Service.IService
{
    public interface ITaskService
    {
        Task<PagedResultDto<TaskDto>> GetAllTasksAsync(string? status,
     string? assignedToUserId,
     string? teamId,
     DateTime? dueDate,
     int? pageNumber,
     int? pageSize,
     string? sortBy,
     bool sortDesc);

        Task<TaskEntity> GetById(string id);
        Task<IEnumerable<TaskEntity>> GetAll();
        IEnumerable<TaskResponse> GetAllTasks(Expression<Func<TaskEntity, bool>>? filter = null, Func<IQueryable<TaskEntity>, IOrderedQueryable<TaskEntity>>? orderBy = null);
        void Add(TaskModel entity);
        void Update(TaskUpdateRequestDto entity);
        void Delete(TaskEntity entity);
        Task<TaskDto?> GetTaskByIdAsync(string id);
        Task<TaskEntity> GetTaskByTitleAsync(string name);
    }
}
