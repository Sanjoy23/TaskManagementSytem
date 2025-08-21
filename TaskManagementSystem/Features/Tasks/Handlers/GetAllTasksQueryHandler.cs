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
    public class GetAllTasksQueryHandler : IRequestHandler<GetAllTasksQuery, PagedResult<TaskResponse>>
    {
        private readonly ITaskService _taskService;

        public GetAllTasksQueryHandler(ITaskService taskService)
        {
            _taskService = taskService;
        }

        public async Task<PagedResult<TaskResponse>> Handle(GetAllTasksQuery request, CancellationToken cancellationToken)
        {


            var tasks = _taskService.GetAllTasksAsync(request.FilterParams);

            return await tasks;
        }
    }
}
