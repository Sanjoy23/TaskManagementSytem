using Application.Models;
using Domain.Entities;
using Domain.Interface;
using MediatR;


namespace Application.Features
{
    public class CreateTaskCommandHandler : IRequestHandler<CreateTaskCommand, CreateTaskResult>
    {
        private readonly ITaskRepository _taskRepository;

        public CreateTaskCommandHandler(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public async Task<CreateTaskResult> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var taskExist = await _taskRepository.GetByTitleAsync(request.Title);
                if (taskExist != null)
                {
                    return new CreateTaskResult
                    {
                        Status = false,
                        Message = "Same Task is created already"
                    };
                }
                var taskModel = new TaskEntity
                {
                    Id = Guid.NewGuid().ToString(),
                    Title = request.Title,
                    Description = request.Description,
                    StatusId = request.StatusId,
                    AssignedToUserId = request.AssignedToUserId,
                    CreatedByUserId = request.CreatedByUserId,
                    TeamId = request.TeamId,
                    DueDate = request.DueDate
                };
                _taskRepository.Add(taskModel);

                return new CreateTaskResult
                {
                    Status = true,
                    Message = "Successfully Created."
                };
            }
            catch (Exception ex)
            {
                //Log.Error(ex, ex.Message); 
                return new CreateTaskResult
                {
                    Status = false,
                    Message = ex.Message
                };
            }
        }
    }
}
