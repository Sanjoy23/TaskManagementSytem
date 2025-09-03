using MediatR;
using TaskManagementSystem.Features.Users.Queries;
using TaskManagementSystem.Models.DTOs;
using TaskManagementSystem.Repository.IRepository;

namespace TaskManagementSystem.Features.Users.Handlers
{
    public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, GetUserByIdResponse>
    {
        private readonly IUserRepository _userRepository;

        public GetUserByIdQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<GetUserByIdResponse> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            var result = await _userRepository.GetById(request.UserId);
            if (result == null)
            {
                return new GetUserByIdResponse
                {
                    Status = false,
                    Message = "User not found"
                };
            }
            return new GetUserByIdResponse
            {
                Status = true,
                Message = "User found successfully.",
                User = new UserDto()
                {
                    Name = result.FullName,
                    Email = result.Email,
                    RoleId = result.RoleId,
                    Role = result.Role,
                    AssignedTasks = result.AssignedTasks,
                    CreatedTasks = result.CreatedTasks,
                }
            };
        }
    }
}
