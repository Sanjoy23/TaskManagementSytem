using MediatR;

namespace TaskManagementSystem.Features.Teams.Commands
{
    public class UpdateTeamCommand : IRequest<UpdateTeamResponse>
    {
        public string TeamId { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }

    public class UpdateTeamResponse
    {
        public bool Status { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
