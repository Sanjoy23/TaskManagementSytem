using MediatR;

namespace TaskManagementSystem.Features.Users.Commands
{
    public class UpdateUserCommand: IRequest<UserUpdateResponse>
    {
        public string Id { get; set; } = null!;
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string RoleId { get; set; } = string.Empty;
    }
    public class UserUpdateResponse
    {
        public bool Status { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
