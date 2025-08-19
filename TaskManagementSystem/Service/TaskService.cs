using Serilog;
using System.Collections;
using System.Linq.Expressions;
using TaskManagementSystem.Models;
using TaskManagementSystem.Models.DTOs;
using TaskManagementSystem.Models.ResponseDtos;
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

        public IEnumerable<TaskResponse> GetAllTasks(
    Expression<Func<TaskEntity, bool>>? filter = null,
    Func<IQueryable<TaskEntity>, IOrderedQueryable<TaskEntity>>? orderBy = null)
        {
            return _taskRepository.GetAll(
                filter: filter,
                includeProperties: "Status,AssignedToUser,CreatedByUser,Team", // eager loading if needed
                selector: x => new TaskResponse
                {
                    Id = x.Id,
                    Title = x.Title,
                    Description = x.Description,
                    StatusId = x.StatusId,
                    AssignedToUserId = x.AssignedToUserId,
                    CreatedByUserId = x.CreatedByUserId,
                    TeamId = x.TeamId,
                    DueDate = x.DueDate
                },
                orderBy: orderBy
            );
        }


        public async Task<PagedResultDto<TaskDto>> GetAllTasksAsync(string? status,
     string? assignedToUserId,
     string? teamId,
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
                    Status = t.StatusId,
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

        public async Task<TaskDto?> GetTaskByIdAsync(string id)
        {
            var task = await _taskRepository.GetByIdAsync(id);
            if (task == null) return null;
            return new TaskDto
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                Status = task.StatusId,
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

        public async Task<TaskEntity> GetTaskByTitleAsync(string name)
        {
            return await _taskRepository.GetByTitleAsync(name);

        }

        public async Task<TaskEntity> GetById(string id)
        {
            return await _taskRepository.GetById(id);
        }

        public async Task<IEnumerable<TaskEntity>> GetAll()
        {
            return await _taskRepository.GetAll();
        }

        public void Add(TaskModel taskModel)
        {
            var taskEntity = new TaskEntity
            {
                Id = Guid.NewGuid().ToString(),
                Title = taskModel.Title,
                Description = taskModel.Description,
                StatusId = taskModel.StatusId,
                AssignedToUserId = taskModel.AssignedToUserId,
                CreatedByUserId = taskModel.CreatedByUserId,
                TeamId = taskModel.TeamId,
                DueDate = taskModel.DueDate
            };
            _taskRepository.Add(taskEntity);
        }

        public void Update(TaskUpdateRequestDto entity)
        {
            try
            {
                var task = _taskRepository.GetById(entity.Id);
                if (task == null)
                {
                    throw new KeyNotFoundException();
                }
                task.Result.Title = entity.Title ?? task.Result.Title;
                task.Result.Description = entity.Description ?? task.Result.Description;
                task.Result.AssignedToUserId = entity.AssignedToUserId ?? task.Result.AssignedToUserId;
                task.Result.CreatedByUserId = entity.CreatedByUserId ?? task.Result.CreatedByUserId;
                task.Result.DueDate = entity.DueDate;
                task.Result.TeamId = entity.TeamId ?? task.Result.TeamId;
                task.Result.StatusId = entity.StatusId ?? task.Result.StatusId;

                _taskRepository.Update(task.Result);
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
                throw;
            }
        }

        public void Delete(TaskEntity entity)
        {
            _taskRepository.Delete(entity);
        }
    }
}
