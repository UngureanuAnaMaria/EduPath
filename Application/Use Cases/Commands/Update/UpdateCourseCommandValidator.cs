using FluentValidation;

namespace Application.Use_Cases.Commands.Update
{
    public class UpdateCourseCommandValidator : AbstractValidator<UpdateCourseCommand>
    {
        public UpdateCourseCommandValidator()
        {
            RuleFor(x => x.Name)
               .NotEmpty().WithMessage("Course name is required.")
               .MaximumLength(200).WithMessage("Course name must not exceed 200 characters.");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Course description is required.")
                .MaximumLength(500).WithMessage("Course description must not exceed 500 characters.");

            RuleFor(x => x.Id)
                .NotEmpty().Must(BeAValidGuid).WithMessage("'Id' must be a valid Guid.");
        }
        private bool BeAValidGuid(Guid guid)
        {
            return Guid.TryParse(guid.ToString(), out _);
        }
    }
}
