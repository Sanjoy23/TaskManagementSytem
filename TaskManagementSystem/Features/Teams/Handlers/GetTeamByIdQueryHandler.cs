using MediatR;
using TaskManagementSystem.Features.Teams.Queries;
using TaskManagementSystem.Models.DTOs;
using TaskManagementSystem.Repository.IRepository;

namespace TaskManagementSystem.Features.Teams.Handlers
{
    public class GetTeamByIdQueryHandler : IRequestHandler<GetTeamByIdQuery, GetTeamByIdResponse>
    {
        private readonly ITeamRepository _teamRepository;

        public GetTeamByIdQueryHandler(ITeamRepository teamRepository)
        {
            _teamRepository = teamRepository;
        }

        public async Task<GetTeamByIdResponse> Handle(GetTeamByIdQuery request, CancellationToken cancellationToken)
        {
            var teamExist = await _teamRepository.GetById(request.TeamId);
            if (teamExist == null)
            {
                return new GetTeamByIdResponse
                {
                    Status = false,
                    Message = "Team not found."
                };
            }
            return new GetTeamByIdResponse
            {
                Status = true,
                Message = "Team found.",
                Team = new TeamDto
                {
                    Id = teamExist.Id,
                    Name = teamExist.Name,
                    Description = teamExist.Description,
                }
            };
        }
    }
}
