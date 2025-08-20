using MediatR;
using TaskManagementSystem.Models.DTOs;
using TaskManagementSystem.Models.ResponseDtos;

namespace TaskManagementSystem.Features.Tasks.Queries
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
