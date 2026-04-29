using Application.Models;
using Domain.Interface;
using MediatR;


namespace Application.Features
{
    public class GetAllTasksQueryHandler : IRequestHandler<GetAllTasksQuery, PagedResult<TaskResponse>>
    {
        private readonly ITaskRepository _taskRepository;

        public GetAllTasksQueryHandler(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public async Task<PagedResult<TaskResponse>> Handle(GetAllTasksQuery request, CancellationToken cancellationToken)
        {


            var tasks = _taskRepository.GetAll(request.FilterParams);

            return await tasks;
        }
    }
}
