using MediatR;
using TaskManagementSystem.Features.Teams.Commands;
using TaskManagementSystem.Repository.IRepository;

namespace TaskManagementSystem.Features.Teams.Handlers
{
    public class DeleteTeamCommandHandler : IRequestHandler<DeleteTeamCommand, DeleteTeamResponse>
    {
        private readonly ITeamRepository _teamRepository;

        public DeleteTeamCommandHandler(ITeamRepository teamRepository)
        {
            _teamRepository = teamRepository;
        }

        public async Task<DeleteTeamResponse> Handle(DeleteTeamCommand request, CancellationToken cancellationToken)
        {
            var teamExist = await _teamRepository.GetById(request.Id);
            if (teamExist == null)
            {
                return new DeleteTeamResponse
                {
                    Status = false,
                    Message = "Team does not exist. Failed to delete."
                };
            }
            _teamRepository.Delete(teamExist);
            return new DeleteTeamResponse
            {
                Status = true,
                Message = "Team deleted successfully."
            };
        }
    }
}
