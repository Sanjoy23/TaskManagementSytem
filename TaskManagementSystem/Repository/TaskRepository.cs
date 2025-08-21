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

        public async Task<TaskEntity?> GetByIdAsync(string id)
        {
            return await _context.Tasks
                .Include(t => t.AssignedToUser)
                .Include(t => t.CreatedByUser)
                .Include(t => t.Team)
                .FirstOrDefaultAsync(t => t.Id == id);
        }
        public async Task<TaskEntity?> GetByTitleAsync(string title)
        {
            return await _context.Tasks.FirstOrDefaultAsync(t => t.Title == title);
        }
    }
}
