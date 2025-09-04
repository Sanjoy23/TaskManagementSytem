using MediatR;
using TaskManagementSystem.Features.Teams.Queries;
using TaskManagementSystem.Models.DTOs;
using TaskManagementSystem.Repository.IRepository;

namespace TaskManagementSystem.Features.Teams.Handlers
{
    public class GetAllTeamQueryHandler : IRequestHandler<GetAllTeamQuery, GetAllTeamsRespnse>
    {
        private readonly ITeamRepository _teamRepository;

        public GetAllTeamQueryHandler(ITeamRepository teamRepository)
        {
            _teamRepository = teamRepository;
        }

        public async Task<GetAllTeamsRespnse> Handle(GetAllTeamQuery request, CancellationToken cancellationToken)
        {
            var result = await _teamRepository.GetAll();
            if (result == null)
            {
                return new GetAllTeamsRespnse
                {
                    Status = false,
                    Message = "No team found.",
                    Teams = new List<TeamDto>()
                };
            }

            var teamList = new List<TeamDto>();
            foreach (var team in result)
            {
                teamList.Add(new TeamDto
                {
                    Id = team.Id,
                    Name = team.Name,
                    Description = team.Description,
                });
            }

            return new GetAllTeamsRespnse
            {
                Status = true,
                Message = "Team list retrive successfully",
                Teams = teamList
            };

        }
    }
}
