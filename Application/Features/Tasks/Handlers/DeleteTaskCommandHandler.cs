using Domain.Interface;
using MediatR;

namespace Application.Features
{
    public class DeleteTaskCommandHandler : IRequestHandler<DeleteTaskCommand, DeleteTaskResult>
    {
        private readonly ITeamRepository _teamRepository;

        public DeleteTaskCommandHandler(ITeamRepository taskRepository)
        {
            _teamRepository = taskRepository;
        }

        public async Task<DeleteTaskResult> Handle(DeleteTaskCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var existingTask = await _teamRepository.GetById(request.TaskId);
                if (existingTask == null)
                {
                    return new DeleteTaskResult
                    {
                        Status = false,
                        Message = "Task not found"
                    };
                }

                _teamRepository.Delete(existingTask);

                return new DeleteTaskResult
                {
                    Status = true,
                    Message = "Task deleted successfully."
                };
            }
            catch (Exception ex)
            {
                //Log.Error(ex, ex.Message);
                return new DeleteTaskResult
                {
                    Status = false,
                    Message = "Error deleting task: " + ex.Message
                };
            }
        }
    }
}
