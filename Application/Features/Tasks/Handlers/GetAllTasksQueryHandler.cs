using Application.Models;
using Domain.Interface;
using Domain.Specifications.Tasks;
using MediatR;


namespace Application.Features
{
    public class GetAllTasksQueryHandler : IRequestHandler<GetAllTasksQuery, Pagination<TaskResponse>>
    {
        private readonly ITaskRepository _taskRepository;

        public GetAllTasksQueryHandler(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public async Task<Pagination<TaskResponse>> Handle(GetAllTasksQuery request, CancellationToken cancellationToken)
        {
            var spec = new TaskSpecification(request.FilterParams);

            var tasks = await _taskRepository.GetAsync(spec);

            var result = tasks.Select(x => new TaskResponse
            {
                Id = x.Id,
                Title = x.Title,
                Description = x.Description,
                Status = x.Status,
                DueDate = x.DueDate,
                AssignedToUser = new UserDto
                {
                    Name = x.AssignedToUser.FullName
                },
                CreatedByUser = new UserDto { 
                    Name = x.CreatedByUser.FullName
                },
                

            });

            return  tasks;
        }
    }
}
