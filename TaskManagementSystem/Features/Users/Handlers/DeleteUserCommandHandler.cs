using MediatR;
using TaskManagementSystem.Features.Users.Commands;
using TaskManagementSystem.Repository.IRepository;

namespace TaskManagementSystem.Features.Users.Handlers
{
    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, UserDeleteResponse>
    {
        private readonly IUserRepository _userRepository;

        public DeleteUserCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<UserDeleteResponse> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var UserExist = await _userRepository.GetById(request.Id);
            if (UserExist == null)
            {
                return new UserDeleteResponse
                {
                    Status = false,
                    Message = "Failed to delete user as User does not exist in the system."
                };
            }
            _userRepository.Delete(UserExist);
            return new UserDeleteResponse { 
                Status = true,
                Message = "User deleted successfully."
            };
        }
    }
}
