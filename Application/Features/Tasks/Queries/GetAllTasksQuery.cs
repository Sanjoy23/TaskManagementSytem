using Application.Models;
using MediatR;

namespace Application.Features
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
