using MediatR;
using TaskManagementSystem.Models;
using TaskManagementSystem.Models.DTOs;
using TaskManagementSystem.Models.ResponseDtos;

namespace TaskManagementSystem.Features.Tasks.Queries
{
    public class GetAllTasksQuery : IRequest<PagedResult<TaskResponse>>
    {
        public TaskFilterParameters FilterParams { get; set; }
        public GetAllTasksQuery(TaskFilterParameters filterParams)
        {
            FilterParams = filterParams;
        }
    }
}
