using MediatR;

namespace TaskManagementSystem.Features.Teams.Commands
{
    public class DeleteTeamCommand : IRequest<DeleteTeamResponse>
    {
        public string Id { get; set; } = null!;
    }
    public class DeleteTeamResponse
    {
        public bool Status { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
