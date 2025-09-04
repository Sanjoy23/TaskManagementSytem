using MediatR;
using TaskManagementSystem.Models.DTOs;

namespace TaskManagementSystem.Features.Teams.Queries
{
    public class GetTeamByIdQuery : IRequest<GetTeamByIdResponse>
    {
        public GetTeamByIdQuery(string teamId)
        {
            TeamId = teamId;
        }

        public string TeamId { get; set; } = string.Empty;
    }

    public class GetTeamByIdResponse
    {
        public bool Status { get; set; }
        public string Message { get; set; } = string.Empty;
        public TeamDto? Team { get; set; }
    }
}
