using Domain.Entities;

namespace Domain.Interface
{
    public interface ITaskRepository : IRepository<TaskEntity>
    {
        Task<TaskEntity?> GetByIdAsync(string id);
        Task<TaskEntity?> GetByTitleAsync(string title);
    }
}
