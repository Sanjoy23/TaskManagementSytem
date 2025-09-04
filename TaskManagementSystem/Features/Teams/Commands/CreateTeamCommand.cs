using MediatR;
namespace TaskManagementSystem.Features.Teams.Commands
{
    public class CreateTeamCommand : IRequest<CreateTeamResponse>
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
    public class CreateTeamResponse
    {
        public bool Status { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
