using Application.Models;
using MediatR;

namespace Application.Features
{
    public class GetTaskByIdQuery: IRequest<TaskResponse>
    {
        public string TaskId { get; set; } = string.Empty;
        public GetTaskByIdQuery(string taskId)
        {
            TaskId = taskId;
        }
    }
}
