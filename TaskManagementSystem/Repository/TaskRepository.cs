using Microsoft.EntityFrameworkCore;
using TaskManagementSystem.Data;
using TaskManagementSystem.Models;
using TaskManagementSystem.Repository.IRepository;
using ModelTask = TaskManagementSystem.Models.Task;

namespace TaskManagementSystem.Repository
{
    public class TaskRepository : ITaskRepository
    {
        private readonly AppDbContext _context;

        public TaskRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<PagedResult<ModelTask>> GetAllAsync(string? status,
    int? assignedToUserId,
    int? teamId,
    DateTime? dueDate,
    int? pageNumber,
    int? pageSize,
    string? sortBy,
    bool sortDesc)
        {
            IQueryable<ModelTask> query = _context.Tasks
        .Include(t => t.AssignedToUser)
        .Include(t => t.CreatedByUser)
        .Include(t => t.Team);
            if (!string.IsNullOrEmpty(status))
                query = query.Where(t => t.Status == status);

            if (assignedToUserId.HasValue)
                query = query.Where(t => t.AssignedToUserId == assignedToUserId.Value);

            if (teamId.HasValue)
                query = query.Where(t => t.TeamId == teamId.Value);

            if (dueDate.HasValue)
                query = query.Where(t => t.DueDate.Date <= dueDate.Value.Date);


            var totalCount = await query.CountAsync();

            // Sorting — apply only if sortBy provided
            if (!string.IsNullOrEmpty(sortBy))
            {
                var propertyInfo = typeof(ModelTask).GetProperty(sortBy);
                if (propertyInfo != null)
                {
                    query = sortDesc
                        ? query.OrderByDescending(t => propertyInfo.GetValue(t, null))
                        : query.OrderBy(t => propertyInfo.GetValue(t, null));
                }
                // If invalid sortBy, you can skip sorting or throw error. Here we skip sorting.
            }

            // Pagination — apply only if both pageNumber and pageSize are provided and > 0
            if (pageNumber.HasValue && pageSize.HasValue && pageNumber > 0 && pageSize > 0)
            {
                query = query.Skip((pageNumber.Value - 1) * pageSize.Value)
                             .Take(pageSize.Value);
            }

            var items = await query.ToListAsync();

            return new PagedResult<ModelTask>
            {
                CurrentPage = pageNumber ?? 1,
                PageSize = pageSize ?? items.Count,
                TotalCount = totalCount,
                Items = items
            };
        }

        public async Task<ModelTask?> GetByIdAsync(int id)
        {
            return await _context.Tasks
                .Include(t => t.AssignedToUser)
                .Include(t => t.CreatedByUser)
                .Include(t => t.Team)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<ModelTask> AddAsync(ModelTask task)
        {
            await _context.Tasks.AddAsync(task);
            return task;
        }

        public async System.Threading.Tasks.Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<ModelTask> UpdateAsync(ModelTask task)
        {
            _context.Tasks.Update(task);
            return task;
        }

        public async System.Threading.Tasks.Task DeleteAsync(ModelTask task)
        {
            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();

        }
    }
}
