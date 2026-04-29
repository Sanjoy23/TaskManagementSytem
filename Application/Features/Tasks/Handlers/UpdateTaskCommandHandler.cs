using Application.Models;
using Domain.Entities;
using Domain.Interface;
using MediatR;

namespace Application.Features
{
    public class UpdateTaskCommandHandler : IRequestHandler<UpdateTaskCommand, UpdateTaskResult>
    {
        private readonly ITaskRepository _taskRepository;

        public UpdateTaskCommandHandler(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public Task<UpdateTaskResult> Handle(UpdateTaskCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var taskEntity = new TaskEntity
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
                _taskRepository.Update(taskEntity);
                return Task.FromResult(new UpdateTaskResult
                {
                    Status = true,
                    Message = "Task updated successfully"
                });
            }
            catch (Exception ex)
            {
                //Log.Error(ex, ex.Message);
                return Task.FromResult(new UpdateTaskResult
                {
                    Status = false,
                    Message = ex.Message
                });
            }
        }
    }
}
