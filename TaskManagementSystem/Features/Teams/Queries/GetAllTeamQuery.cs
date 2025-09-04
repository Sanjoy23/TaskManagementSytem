using MediatR;
using TaskManagementSystem.Models.DTOs;

namespace TaskManagementSystem.Features.Teams.Queries
{
    public class GetAllTeamQuery : IRequest<GetAllTeamsRespnse>
    {

    }

    public class GetAllTeamsRespnse
    {
        public bool Status { get; set; }
        public string Message { get; set; } = string.Empty;
        public List<TeamDto>? Teams { get; set; }
    }
}
