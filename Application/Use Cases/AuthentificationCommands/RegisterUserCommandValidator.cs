using FluentValidation;

namespace Application.Use_Cases.Authentification
{
    public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
    {
        public RegisterUserCommandValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Email is not valid.")
                .MaximumLength(320).WithMessage("Email must not exceed 320 characters.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters long.")
                .MaximumLength(200).WithMessage("Password must not exceed 200 characters.");

            RuleFor(x => x.Admin)
                .NotNull().WithMessage("Admin flag is required.");
        }
    }
}
