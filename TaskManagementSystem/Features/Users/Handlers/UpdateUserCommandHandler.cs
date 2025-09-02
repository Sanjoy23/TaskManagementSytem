using MediatR;
using Microsoft.AspNetCore.Identity;
using TaskManagementSystem.Features.Users.Commands;
using TaskManagementSystem.Models;
using TaskManagementSystem.Repository.IRepository;

namespace TaskManagementSystem.Features.Users.Handlers
{
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, UserUpdateResponse>
    {
        private readonly IUserRepository _userRepository;

        public UpdateUserCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<UserUpdateResponse> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var userExist = await _userRepository.GetById(request.Id);
            if (userExist == null) {
                return new UserUpdateResponse{ 
                    Status = false,
                    Message = "User not found."
                };
            }

            userExist.FullName = request.FullName;
            userExist.Email = request.Email;
            userExist.RoleId = request.RoleId;
            _userRepository.Update(userExist);

            return new UserUpdateResponse
            {
                Status = true,
                Message = "User updated successfully."
            };

        }
    }
}
