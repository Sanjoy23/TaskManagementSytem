using Application.Models;
using MediatR;

namespace Application.Features.Users.Queries
{
    public class GetUserByIdQuery : IRequest<GetUserByIdResponse>
    {
        public GetUserByIdQuery(string userId)
        {
            UserId = userId;
        }

        public string UserId { get; set; } = string.Empty;
    }

    public class GetUserByIdResponse
    {
        public bool Status { get; set; }
        public string Message { get; set; } = string.Empty;
        public UserDto? User { get; set; }
    }
}
