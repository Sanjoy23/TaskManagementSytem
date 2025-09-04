using MediatR;
using TaskManagementSystem.Features.Teams.Commands;
using TaskManagementSystem.Models;
using TaskManagementSystem.Repository.IRepository;

namespace TaskManagementSystem.Features.Teams.Handlers
{
    public class CreateTeamCommandHandler : IRequestHandler<CreateTeamCommand, CreateTeamResponse>
    {
        private readonly ITeamRepository _teamRepository;

        public CreateTeamCommandHandler(ITeamRepository teamRepository)
        {
            _teamRepository = teamRepository;
        }

        public async Task<CreateTeamResponse> Handle(CreateTeamCommand request, CancellationToken cancellationToken)
        {
            var teamExist = await _teamRepository.GetByNameAsync(request.Name);
            if (teamExist != null)
            {
                return new CreateTeamResponse
                {
                    Status = false,
                    Message = "Team with same name already exist."
                };
            }
            var newTeam = new Team
            {
                Id = Guid.NewGuid().ToString(),
                Name = request.Name,
                Description = request.Description,
            };

            _teamRepository.Add(newTeam);
            return new CreateTeamResponse
            {
                Status = true,
                Message = "Team created successfully."
            };
        }
    }
}
