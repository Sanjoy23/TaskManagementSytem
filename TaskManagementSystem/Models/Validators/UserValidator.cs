using FluentValidation;

namespace TaskManagementSystem.Models.Validators
{
    public class UserValidator : AbstractValidator<User>
    {
        public UserValidator()
        {
            RuleFor(user => user.FullName)
              .NotEmpty()
              .WithMessage("First name is required.");
            RuleFor(user => user.Email)
              .EmailAddress()
              .WithMessage("Invalid email address.");
            RuleFor(user => user.Role)
              .NotEmpty()
              .WithMessage("First name is required.");
            RuleFor(user => user.Password)
              .NotEmpty()
              .WithMessage("First name is required.");
        }
    }
}
