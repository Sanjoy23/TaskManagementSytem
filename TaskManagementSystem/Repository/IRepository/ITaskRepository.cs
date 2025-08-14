using TaskManagementSystem.Models;
namespace TaskManagementSystem.Repository.IRepository
{
    public interface ITaskRepository : IRepository<TaskEntity>
    {
        Task<PagedResult<TaskEntity>> GetAllAsync(string? status,
            int? assignedToUserId,
            int? teamId,
            DateTime? dueDate,
            int? pageNumber,
            int? pageSize,
            string? sortBy,
            bool sortDesc);
        Task<TaskEntity?> GetByIdAsync(int id);
        Task<TaskEntity?> GetByTitleAsync(string title);
        Task<TaskEntity> AddAsync(TaskEntity task);
        Task<TaskEntity> UpdateAsync(TaskEntity task);
        Task DeleteAsync(TaskEntity task);
        Task SaveChangesAsync();
    }
}
