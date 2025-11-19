using MediatR;
using TaskManagementSystem.Features.Tasks.Queries;
using TaskManagementSystem.Models.DTOs;
using TaskManagementSystem.Models.ResponseDtos;
using TaskManagementSystem.Service.IService;

namespace TaskManagementSystem.Features.Tasks.Handlers
{
    public class GetTaskByIdHandler : IRequestHandler<GetTaskByIdQuery, TaskResponse>
    {
        private readonly ITaskService _taskService;

        public GetTaskByIdHandler(ITaskService taskService)
        {
            _taskService = taskService;
        }

        public async Task<TaskResponse> Handle(GetTaskByIdQuery request, CancellationToken cancellationToken)
        {
            var result = await _taskService.GetById(request.TaskId);
            if (result == null)
                throw new Exception($"Task with ID {request.TaskId} not found");
            return new TaskResponse
            {
                Id = result.Id,
                Title = result.Title,
                Description = result.Description,
                StatusId = result.StatusId,
                Status = result.Status,
                AssignedToUserId = result.AssignedToUserId,
                AssignedToUser = new UserDto
                {
                    Name = result.AssignedToUser.FullName,
                    Email = result.AssignedToUser.Email,
                },
                CreatedByUserId = result.CreatedByUserId,
                CreatedByUser = new UserDto
                {
                    Name = result.CreatedByUser.FullName,
                    Email = result.CreatedByUser.Email
                },
                TeamId = result.TeamId,
                Team = new TeamDto
                {
                    Id = result.Team.Id,
                    Name = result.Team.Name,
                },
                DueDate = result.DueDate
            };
        }
    }
}
