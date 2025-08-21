using System.Linq.Expressions;
using TaskManagementSystem.Models;
using TaskManagementSystem.Models.DTOs;
using TaskManagementSystem.Models.ResponseDtos;
namespace TaskManagementSystem.Service.IService
{
    public interface ITaskService
    {
        Task<TaskEntity> GetById(string id);
        Task<IEnumerable<TaskEntity>> GetAll();
        Task<PagedResult<TaskResponse>> GetAllTasksAsync(TaskFilterParameters filterParams);
        void Add(TaskModel entity);
        void Update(TaskUpdateRequestDto entity);
        void Delete(TaskEntity entity);
        Task<TaskDto?> GetTaskByIdAsync(string id);
        Task<TaskEntity> GetTaskByTitleAsync(string name);
        void UpdateStatus(string userId, string taskId, string statusId);
    }
}
