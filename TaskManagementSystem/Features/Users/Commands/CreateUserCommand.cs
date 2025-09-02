using MediatR;

namespace TaskManagementSystem.Features.Users.Commands
{
    public class CreateUserCommand: IRequest<CreateUserResponse>
    {
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string RoleId { get; set; } = string.Empty;
    }
    public class CreateUserResponse
    {
        public bool Status { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
