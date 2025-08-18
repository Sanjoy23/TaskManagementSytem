using Microsoft.EntityFrameworkCore;
using TaskManagementSystem.Data;
using TaskManagementSystem.Models;
using TaskManagementSystem.Repository.IRepository;

namespace TaskManagementSystem.Repository
{
    public class TaskRepository : Repository<TaskEntity>, ITaskRepository
    {
        private readonly AppDbContext _context;

        public TaskRepository(AppDbContext context): base(context)
        {
            _context = context;
        }

        public async Task<PagedResult<TaskEntity>> GetAllAsync(string? statusId,
    string? assignedToUserId,
    string? teamId,
    DateTime? dueDate,
    int? pageNumber,
    int? pageSize,
    string? sortBy,
    bool sortDesc)
        {
            IQueryable<TaskEntity> query = _context.Tasks
        .Include(t => t.AssignedToUser)
        .Include(t => t.CreatedByUser)
        .Include(t => t.Team);
            if (!string.IsNullOrEmpty(statusId))
                query = query.Where(t => t.StatusId == statusId);

            if (!string.IsNullOrEmpty(assignedToUserId))
                query = query.Where(t => t.AssignedToUserId == assignedToUserId);

            if (!string.IsNullOrEmpty(teamId))
                query = query.Where(t => t.TeamId == teamId);

            if (dueDate.HasValue)
                query = query.Where(t => t.DueDate.Date <= dueDate.Value.Date);


            var totalCount = await query.CountAsync();

            // Sorting — apply only if sortBy provided
            if (!string.IsNullOrEmpty(sortBy))
            {
                query = sortDesc
                    ? query.OrderByDescending(e => EF.Property<object>(e, sortBy))
                    : query.OrderBy(e => EF.Property<object>(e, sortBy));
            }

            // Pagination — apply only if both pageNumber and pageSize are provided and > 0
            if (pageNumber.HasValue && pageSize.HasValue && pageNumber > 0 && pageSize > 0)
            {
                query = query.Skip((pageNumber.Value - 1) * pageSize.Value)
                             .Take(pageSize.Value);
            }

            var items = await query.ToListAsync();

            return new PagedResult<TaskEntity>
            {
                CurrentPage = pageNumber ?? 1,
                PageSize = pageSize ?? items.Count,
                TotalCount = totalCount,
                Items = items
            };
        }

        public async Task<TaskEntity?> GetByIdAsync(string id)
        {
            return await _context.Tasks
                .Include(t => t.AssignedToUser)
                .Include(t => t.CreatedByUser)
                .Include(t => t.Team)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<TaskEntity> AddAsync(TaskEntity task)
        {
            await _context.Tasks.AddAsync(task);
            await _context.SaveChangesAsync();
            return task;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<TaskEntity> UpdateAsync(TaskEntity task)
        {
            _context.Tasks.Update(task);
            return task;
        }

        public async Task DeleteAsync(TaskEntity task)
        {
            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();

        }

        public async Task<TaskEntity?> GetByTitleAsync(string title)
        {
            return await _context.Tasks.FirstOrDefaultAsync(t => t.Title == title);
        }
    }
}
