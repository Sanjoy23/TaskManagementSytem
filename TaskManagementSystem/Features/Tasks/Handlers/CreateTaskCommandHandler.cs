using MediatR;
using Serilog;
using TaskManagementSystem.Features.Tasks.Commands;
using TaskManagementSystem.Models;
using TaskManagementSystem.Service.IService;

namespace TaskManagementSystem.Features.Tasks.Handlers
{
    public class CreateTaskCommandHandler : IRequestHandler<CreateTaskCommand, CreateTaskResult>
    {
        private readonly ITaskService _taskService;

        public CreateTaskCommandHandler(ITaskService taskService)
        {
            _taskService = taskService;
        }

        public async Task<CreateTaskResult> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var taskExist = await _taskService.GetTaskByTitleAsync(request.Title);
                if (taskExist != null)
                {
                    return new CreateTaskResult
                    {
                        Status = false,
                        Message = "Same Task is created already"
                    };
                }
                var taskModel = new TaskModel
                {
                    Title = request.Title,
                    Description = request.Description,
                    StatusId = request.StatusId,
                    AssignedToUserId = request.AssignedToUserId,
                    CreatedByUserId = request.CreatedByUserId,
                    TeamId = request.TeamId,
                    DueDate = request.DueDate
                };
                _taskService.Add(taskModel);

                return new CreateTaskResult
                {
                    Status = true,
                    Message = "Successfully Created."
                };
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message); 
                return new CreateTaskResult
                {
                    Status = false,
                    Message = ex.Message
                };
            }
        }
    }
}
