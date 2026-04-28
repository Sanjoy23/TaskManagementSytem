using MediatR;

namespace Application.Features
{
    public class DeleteTaskCommand : IRequest<DeleteTaskResult>
    {
        public string TaskId { get; set; } = string.Empty;
    }

    public class DeleteTaskResult
    {
        public bool Status { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
