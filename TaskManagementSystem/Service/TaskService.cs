using Serilog;
using System.Collections;
using System.Linq.Expressions;
using TaskManagementSystem.Models;
using TaskManagementSystem.Models.DTOs;
using TaskManagementSystem.Models.ResponseDtos;
using TaskManagementSystem.Repository;
using TaskManagementSystem.Repository.IRepository;
using TaskManagementSystem.Service.IService;

namespace TaskManagementSystem.Service
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IUserRepository _userRepository;

        public TaskService(ITaskRepository taskRepository, IUserRepository userRepository)
        {
            _taskRepository = taskRepository;
            _userRepository = userRepository;
        }

        public async Task<PagedResult<TaskResponse>> GetAllTasksAsync(TaskFilterParameters filterParams)
        {
            Expression<Func<TaskEntity, bool>> filter = BuildFilter(filterParams);
            var orderBy = BuildOrderBy(filterParams.SortBy, filterParams.SortDirection);

            return await _taskRepository.GetPagedAsync(
                filter: filter,
                includeProperties: "Status,AssignedToUser,CreatedByUser,Team",
                selector: x => new TaskResponse
                {
                    Id = x.Id,
                    Title = x.Title,
                    Description = x.Description,
                    StatusId = x.StatusId,
                    AssignedToUserId = x.AssignedToUserId,
                    CreatedByUserId = x.CreatedByUserId,
                    TeamId = x.TeamId,
                    DueDate = x.DueDate,
                    Status = x.Status != null ? new TasksStatus
                    {
                        Id = x.Status.Id,
                        Name = x.Status.Name
                    } : null,
                    AssignedToUser = x.AssignedToUser != null ? new UserDto
                    {
                        Name = x.AssignedToUser.FullName,
                        Email = x.AssignedToUser.Email
                    } : null,
                    CreatedByUser = x.CreatedByUser != null ? new UserDto
                    {
                        Name = x.CreatedByUser.FullName,
                        Email = x.CreatedByUser.Email
                    } : null,
                    Team = x.Team != null ? new TeamDto
                    {
                        Id = x.Team.Id,
                        Name = x.Team.Name
                    } : null
                },
                        orderBy: orderBy,
                        pageNumber: filterParams.PageNumber,
                        pageSize: filterParams.PageSize
            );
        }


        private Expression<Func<TaskEntity, bool>> BuildFilter(TaskFilterParameters filterParams)
        {
            var parameter = Expression.Parameter(typeof(TaskEntity), "t");
            Expression? body = null;

            // Status filter
            if (filterParams.Statuses?.Any() == true)
            {
                var statusProperty = Expression.Property(parameter, "Status");
                var statusNameProperty = Expression.Property(statusProperty, "Name");
                var statusValues = Expression.Constant(filterParams.Statuses);
                var statusContains = Expression.Call(statusValues, "Contains", null, statusNameProperty);
                var statusNotNull = Expression.NotEqual(statusProperty, Expression.Constant(null));
                var statusCondition = Expression.AndAlso(statusNotNull, statusContains);

                body = body == null ? statusCondition : Expression.AndAlso(body, statusCondition);
            }

            // Assigned user filter
            if (filterParams.AssignedToUserIds?.Any() == true)
            {
                var userIdProperty = Expression.Property(parameter, "AssignedToUserId");
                var userIdValues = Expression.Constant(filterParams.AssignedToUserIds);
                var userIdContains = Expression.Call(userIdValues, "Contains", null, userIdProperty);

                body = body == null ? userIdContains : Expression.AndAlso(body, userIdContains);
            }

            // Team filter
            if (filterParams.TeamIds?.Any() == true)
            {
                var teamProperty = Expression.Property(parameter, "Team");
                var teamNameProperty = Expression.Property(teamProperty, "Name");
                var teamValues = Expression.Constant(filterParams.TeamIds);
                var teamContains = Expression.Call(teamValues, "Contains", null, teamNameProperty);
                var teamNotNull = Expression.NotEqual(teamProperty, Expression.Constant(null));
                var teamCondition = Expression.AndAlso(teamNotNull, teamContains);

                body = body == null ? teamCondition : Expression.AndAlso(body, teamCondition);
            }

            // Date range filter (improved from single date)
            if (filterParams.DueDateFrom.HasValue)
            {
                var dueDateProperty = Expression.Property(parameter, "DueDate");
                var dueDateFrom = Expression.Constant(filterParams.DueDateFrom.Value.Date);
                var dateCondition = Expression.GreaterThanOrEqual(dueDateProperty, dueDateFrom);

                body = body == null ? dateCondition : Expression.AndAlso(body, dateCondition);
            }

            if (filterParams.DueDateTo.HasValue)
            {
                var dueDateProperty = Expression.Property(parameter, "DueDate");
                var dueDateTo = Expression.Constant(filterParams.DueDateTo.Value.Date.AddDays(1));
                var dateCondition = Expression.LessThan(dueDateProperty, dueDateTo);

                body = body == null ? dateCondition : Expression.AndAlso(body, dateCondition);
            }

            // Search term filter
            if (!string.IsNullOrWhiteSpace(filterParams.SearchTerm))
            {
                var titleProperty = Expression.Property(parameter, "Title");
                var descProperty = Expression.Property(parameter, "Description");
                var searchTerm = Expression.Constant(filterParams.SearchTerm.ToLower());

                var titleContains = Expression.Call(
                    Expression.Call(titleProperty, "ToLower", null),
                    "Contains", null, searchTerm);
                var descContains = Expression.Call(
                    Expression.Call(descProperty, "ToLower", null),
                    "Contains", null, searchTerm);

                var searchCondition = Expression.OrElse(titleContains, descContains);
                body = body == null ? searchCondition : Expression.AndAlso(body, searchCondition);
            }

            return body != null ? Expression.Lambda<Func<TaskEntity, bool>>(body, parameter) : (t => true);
        }
        private Func<IQueryable<TaskEntity>, IOrderedQueryable<TaskEntity>>? BuildOrderBy(string? sortBy, string? sortDirection)
        {
            if (string.IsNullOrEmpty(sortBy)) return null;

            var isDescending = sortDirection?.ToLower() == "desc";

            return sortBy.ToLower() switch
            {
                "title" => q => isDescending ? q.OrderByDescending(t => t.Title) : q.OrderBy(t => t.Title),
                "duedate" => q => isDescending ? q.OrderByDescending(t => t.DueDate) : q.OrderBy(t => t.DueDate),
                "status" => q => isDescending ? q.OrderByDescending(t => t.Status!.Name) : q.OrderBy(t => t.Status!.Name),
                _ => q => q.OrderByDescending(t => t.DueDate) // Default
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

        public void UpdateStatus(string userId, string taskId, string statusId)
        {
            var task = _taskRepository.GetById(taskId).Result;
            var user = _userRepository.GetById(userId).Result;
            if (task == null)
                throw new KeyNotFoundException("Task not found.");

            if (!string.Equals(user.Id, task.AssignedToUserId, StringComparison.OrdinalIgnoreCase))
                throw new UnauthorizedAccessException("Task does not belong to this user.");

            task.StatusId = statusId;
            _taskRepository.Update(task);
        }
    }
}
