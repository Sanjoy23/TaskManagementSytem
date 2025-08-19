using MediatR;

namespace TaskManagementSystem.Features.Tasks.Commands
{
    public class UpdateTaskCommand : IRequest<UpdateTaskResult>
    {
        public string Id { get; set; } = null!;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string StatusId { get; set; } = string.Empty;
        public string AssignedToUserId { get; set; } = null!;
        public string CreatedByUserId { get; set; } = null!;
        public string TeamId { get; set; } = null!;
        public DateTime DueDate { get; set; }
    }

    public class UpdateTaskResult
    {
        public bool Status { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
