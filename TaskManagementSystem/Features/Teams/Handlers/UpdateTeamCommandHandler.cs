using MediatR;
using TaskManagementSystem.Features.Teams.Commands;
using TaskManagementSystem.Repository.IRepository;

namespace TaskManagementSystem.Features.Teams.Handlers
{
    public class UpdateTeamCommandHandler : IRequestHandler<UpdateTeamCommand, UpdateTeamResponse>
    {
        private readonly ITeamRepository _teamRepository;

        public UpdateTeamCommandHandler(ITeamRepository teamRepository)
        {
            _teamRepository = teamRepository;
        }

        public async Task<UpdateTeamResponse> Handle(UpdateTeamCommand request, CancellationToken cancellationToken)
        {
            var teamExist = await _teamRepository.GetById(request.TeamId);
            if (teamExist == null)
            {
                return new UpdateTeamResponse
                {
                    Status = false,
                    Message = "Team does not exist in the system."
                };
            }
            teamExist.Name = request.Name;
            teamExist.Description = request.Description;
            _teamRepository.Update(teamExist);

            return new UpdateTeamResponse
            {
                Status = true,
                Message = "Team updated successfully."
            };
        }
    }
}
