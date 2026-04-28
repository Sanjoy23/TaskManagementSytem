using Application.Models;
using MediatR;

namespace Application.Features.Teams.Queries
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
