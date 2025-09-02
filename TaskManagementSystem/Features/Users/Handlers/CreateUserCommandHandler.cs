using MediatR;
using Microsoft.AspNetCore.Identity;
using TaskManagementSystem.Features.Users.Commands;
using TaskManagementSystem.Models;
using TaskManagementSystem.Repository.IRepository;

namespace TaskManagementSystem.Features.Users.Handlers
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, CreateUserResponse>
    {
        private readonly IUserRepository _userRepository;

        public CreateUserCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<CreateUserResponse> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var userExist = await _userRepository.GetByEmailAsync(request.Email);
            if (userExist != null)
            {
                return new CreateUserResponse
                {
                    Status = false,
                    Message = "User Already Exist"
                };
            }
            var user = new User
            {
                FullName = request.FullName,
                Email = request.Email,
                RoleId = request.RoleId

            };
            var passwordHasher = new PasswordHasher<User>();
            user.Password = passwordHasher.HashPassword(user, request.Password);
            _userRepository.Add(user);
            return new CreateUserResponse
            {
                Status = true,
                Message = "User Created successfully."
            };
        }
    }
}
