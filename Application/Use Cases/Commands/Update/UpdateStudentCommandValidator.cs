using FluentValidation;

namespace Application.Use_Cases.Commands.Update
{
    public class UpdateStudentCommandValidator : AbstractValidator<UpdateStudentCommand>
    {
        public UpdateStudentCommandValidator()
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

            RuleFor(x => x.Id)
                .NotEmpty().Must(BeAValidGuid).WithMessage("'Id' must be a valid Guid.");
        }
        private bool BeAValidGuid(Guid guid)
        {
            return Guid.TryParse(guid.ToString(), out _);
        }
    }
}
