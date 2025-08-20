using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Serilog;
using System.Linq.Expressions;
using TaskManagementSystem.Features.Tasks.Queries;
using TaskManagementSystem.Models;
using TaskManagementSystem.Models.ResponseDtos;
using TaskManagementSystem.Service.IService;

namespace TaskManagementSystem.Features.Tasks.Handlers
{
    public class GetAllTasksQueryHandler : IRequestHandler<GetAllTasksQuery, IEnumerable<TaskResponse>>
    {
        private readonly ITaskService _taskService;

        public GetAllTasksQueryHandler(ITaskService taskService)
        {
            _taskService = taskService;
        }

        public async Task<IEnumerable<TaskResponse>> Handle(GetAllTasksQuery request, CancellationToken cancellationToken)
        {
            var filterParams = request.FilterParams;

            Expression<Func<TaskEntity, bool>>? filter = null;

            if ((filterParams.Statuses != null && filterParams.Statuses.Any()) ||
                (filterParams.AssignedToUserIds != null && filterParams.AssignedToUserIds.Any()) ||
                (filterParams.TeamIds != null && filterParams.TeamIds.Any()) ||
                filterParams.DueDate.HasValue)
            {
                filter = t =>
                    (filterParams.Statuses == null || !filterParams.Statuses.Any() || (t.Status != null && filterParams.Statuses.Contains(t.Status.Name))) &&
                    (filterParams.AssignedToUserIds == null || !filterParams.AssignedToUserIds.Any() || filterParams.AssignedToUserIds.Contains(t.AssignedToUserId)) &&
                    (filterParams.TeamIds == null || !filterParams.TeamIds.Any() || (t.Team != null && filterParams.TeamIds.Contains(t.Team.Name))) &&
                    (!filterParams.DueDate.HasValue || t.DueDate.Date == filterParams.DueDate.Value.Date);
            }

            var tasks = _taskService.GetAllTasks(
                filter: filter,
                orderBy: q => q.OrderByDescending(t => t.DueDate)
            );

            return await Task.FromResult(tasks);
        }
    }
}
