using MediatR;
using TaskManagementSystem.Models.DTOs;

namespace TaskManagementSystem.Features.Users.Queries
{
    public class GetAllUserQuery: IRequest<GetAllUserRespnse>
    {
    }

    public class GetAllUserRespnse
    {
        public bool Status { get; set; }
        public string Message { get; set; } = string.Empty;
        public List<UserDto>? Users { get; set; }
    }
}
