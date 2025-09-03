using MediatR;
using TaskManagementSystem.Features.Users.Queries;
using TaskManagementSystem.Models.DTOs;
using TaskManagementSystem.Repository.IRepository;

namespace TaskManagementSystem.Features.Users.Handlers
{
    public class GetAllUserQueryHandler : IRequestHandler<GetAllUserQuery, GetAllUserRespnse>
    {
        private readonly IUserRepository _userRepository;

        public GetAllUserQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<GetAllUserRespnse> Handle(GetAllUserQuery request, CancellationToken cancellationToken)
        {
            var result = await _userRepository.GetAll();
            var userlist = new List<UserDto>();
            foreach (var user in result)
            {
                var userdto = new UserDto
                {
                    Name = user.FullName,
                    Email = user.Email,
                    RoleId = user.RoleId,
                    Role = user.Role,
                    CreatedTasks = user.CreatedTasks,
                    AssignedTasks = user.AssignedTasks
                };
                userlist.Add(userdto);
            }

            if (result == null)
            {
                return new GetAllUserRespnse
                {
                    Status = false,
                    Message = "No User found.",
                    Users = new List<UserDto>()
                };
            }


            return new GetAllUserRespnse
            {
                Status = true,
                Message = "User list retrive successfully",
                Users = userlist
            };
        }
    }
}
