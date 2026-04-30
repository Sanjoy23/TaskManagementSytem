using Application.Models;
using Domain.Specifications.Tasks;
using MediatR;

namespace Application.Features
{
    public class GetAllTasksQuery : IRequest<Pagination<TaskResponse>>
    {
        public TaskSpecParams FilterParams { get; set; }
        public GetAllTasksQuery(TaskSpecParams taskParams)
        {
            FilterParams = taskParams;
        }
    }
}
