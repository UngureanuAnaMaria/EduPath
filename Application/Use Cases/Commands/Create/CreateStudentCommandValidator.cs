using FluentValidation;

namespace Application.Use_Cases.Commands.Create
{
    public class CreateStudentCommandValidator : AbstractValidator<CreateStudentCommand>
    {
        public CreateStudentCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(200).WithMessage("Name must not exceed 200 characters.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Email is not valid.")
                .MaximumLength(320).WithMessage("Email must not exceed 320 characters.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MaximumLength(200).WithMessage("Password must not exceed 200 characters.");

            RuleFor(x => x.Status)
                .NotNull().WithMessage("Status is required.");

            RuleFor(x => x.CreatedAt)
                .NotEmpty().WithMessage("CreatedAt is required.");

            RuleFor(x => x.LastLogin)
                .NotEmpty().WithMessage("LastLogin is required.");
        }
    }
}
