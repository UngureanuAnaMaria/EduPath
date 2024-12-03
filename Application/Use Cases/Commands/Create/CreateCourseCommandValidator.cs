using FluentValidation;

namespace Application.Use_Cases.Commands.Create
{
    public class CreateCourseCommandValidator : AbstractValidator<CreateCourseCommand>
    {
        public CreateCourseCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Course name is required.")
                .MaximumLength(200).WithMessage("Course name must not exceed 200 characters.");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Course description is required.")
                .MaximumLength(500).WithMessage("Course description must not exceed 500 characters.");
        }
    }
}
