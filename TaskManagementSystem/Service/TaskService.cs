using TaskManagementSystem.Models;
using TaskManagementSystem.Models.DTOs;
using TaskManagementSystem.Repository.IRepository;
using TaskManagementSystem.Service.IService;

namespace TaskManagementSystem.Service
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepository;

        public TaskService(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public async Task<PagedResultDto<TaskDto>> GetAllTasksAsync(string? status,
     int? assignedToUserId,
     int? teamId,
     DateTime? dueDate,
     int? pageNumber,
     int? pageSize,
     string? sortBy,
     bool sortDesc)
        {
            var result = await _taskRepository.GetAllAsync(status, assignedToUserId, teamId, dueDate, pageNumber, pageSize, sortBy, sortDesc);

            return new PagedResultDto<TaskDto>
            {
                CurrentPage = result.CurrentPage,
                PageSize = result.PageSize,
                TotalCount = result.TotalCount,
                Items = result.Items.Select(t => new TaskDto
                {
                    Id = t.Id,
                    Title = t.Title,
                    Description = t.Description,
                    Status = t.Status,
                    AssignedToUserId = t.AssignedToUserId,
                    CreatedByUserId = t.CreatedByUserId,
                    TeamId = t.TeamId,
                    DueDate = t.DueDate,
                    AssignedToUser = t.AssignedToUser != null ? new UserDto
                    {
                        Name = t.AssignedToUser.FullName,
                        Email = t.AssignedToUser.Email
                    } : null,
                    CreatedByUser = t.CreatedByUser != null ? new UserDto
                    {
                        Name = t.CreatedByUser.FullName,
                        Email = t.CreatedByUser.Email
                    } : null,
                    Team = t.Team != null ? new TeamDto
                    {
                        Id = t.Team.Id,
                        Name = t.Team.Name
                    } : null
                }).ToList()
            };
        }

        public async Task<TaskDto?> GetTaskByIdAsync(int id)
        {
            var task = await _taskRepository.GetByIdAsync(id);
            if (task == null) return null;
            return new TaskDto
            {
                Id = task.Id,
                Title = task.Title, 
                Description = task.Description, 
                Status = task.Status,
                AssignedToUserId = task.AssignedToUserId,
                CreatedByUserId = task.CreatedByUserId, 
                TeamId = task.TeamId,
                DueDate = task.DueDate,
                AssignedToUser = new UserDto
                {
                    Email = task.AssignedToUser.Email,
                    Name = task.AssignedToUser.FullName
                },
                CreatedByUser = new UserDto
                {
                    Email = task.CreatedByUser.Email,
                    Name = task.CreatedByUser.FullName
                },
                Team = new TeamDto
                {
                    Id = task.Team.Id,
                    Name = task.Team.Name
                }

            };
        }

        public async Task<TaskEntity> CreateTaskAsync(TaskEntity task)
        {
            var created = await _taskRepository.AddAsync(task);
            return created;
        }

        public async Task<TaskDto?> UpdateTaskAsync(TaskEntity task)
        {
            var updated = await _taskRepository.UpdateAsync(task);
            await _taskRepository.SaveChangesAsync();
            return new TaskDto
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                Status = task.Status,
                AssignedToUserId = task.AssignedToUserId,
                CreatedByUserId = task.CreatedByUserId,
                TeamId = task.TeamId,
                DueDate = task.DueDate,
                AssignedToUser = new UserDto
                {
                    Email = task.AssignedToUser.Email,
                    Name = task.AssignedToUser.FullName
                },
                CreatedByUser = new UserDto
                {
                    Email = task.CreatedByUser.Email,
                    Name = task.CreatedByUser.FullName
                },
                Team = new TeamDto
                {
                    Id = task.Team.Id,
                    Name = task.Team.Name
                }

            };
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

        public async Task<TaskEntity> GetTaskByTitleAsync(string name)
        {
            return await _taskRepository.GetByTitleAsync(name);
            
        }
    }
}
