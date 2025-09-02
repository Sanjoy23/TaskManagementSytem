using MediatR;

namespace TaskManagementSystem.Features.Users.Commands
{
    public class DeleteUserCommand : IRequest<UserDeleteResponse>
    {
        public string Id { get; set; } = null!;
    }

    public class UserDeleteResponse
    {
        public bool Status { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
