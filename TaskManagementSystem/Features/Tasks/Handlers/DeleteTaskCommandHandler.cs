using MediatR;
using Serilog;
using TaskManagementSystem.Features.Tasks.Commands;
using TaskManagementSystem.Service.IService;

namespace TaskManagementSystem.Features.Tasks.Handlers
{
    public class DeleteTaskCommandHandler : IRequestHandler<DeleteTaskCommand, DeleteTaskResult>
    {
        private readonly ITaskService _taskService;

        public DeleteTaskCommandHandler(ITaskService taskService)
        {
            _taskService = taskService;
        }

        public async Task<DeleteTaskResult> Handle(DeleteTaskCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var existingTask = await _taskService.GetById(request.TaskId);
                if (existingTask == null)
                {
                    return new DeleteTaskResult
                    {
                        Status = false,
                        Message = "Task not found"
                    };
                }

                _taskService.Delete(existingTask);

                return new DeleteTaskResult
                {
                    Status = true,
                    Message = "Task deleted successfully."
                };
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
                return new DeleteTaskResult
                {
                    Status = false,
                    Message = "Error deleting task: " + ex.Message
                };
            }
        }
    }
}
