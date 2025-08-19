using MediatR;
using Serilog;
using TaskManagementSystem.Features.Tasks.Commands;
using TaskManagementSystem.Models.DTOs;
using TaskManagementSystem.Service.IService;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace TaskManagementSystem.Features.Tasks.Handlers
{
    public class UpdateTaskCommandHandler : IRequestHandler<UpdateTaskCommand, UpdateTaskResult>
    {
        private readonly ITaskService _taskService;

        public UpdateTaskCommandHandler(ITaskService taskService)
        {
            _taskService = taskService;
        }

        public Task<UpdateTaskResult> Handle(UpdateTaskCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var taskUpdateRequestDto = new TaskUpdateRequestDto
                {
                    Id = request.Id,
                    Title = request.Title,
                    Description = request.Description,
                    StatusId = request.StatusId,
                    AssignedToUserId = request.AssignedToUserId,
                    CreatedByUserId = request.CreatedByUserId,
                    TeamId = request.TeamId,
                    DueDate = request.DueDate,
                };
                _taskService.Update(taskUpdateRequestDto);
                return Task.FromResult(new UpdateTaskResult
                {
                    Status = true,
                    Message = "Task updated successfully"
                });
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
                return Task.FromResult(new UpdateTaskResult
                {
                    Status = false,
                    Message = ex.Message
                });
            }
        }
    }
}
