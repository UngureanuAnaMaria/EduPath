using FluentValidation;

namespace Application.Use_Cases.Commands.Create
{
    public class CreateLessonCommandValidator : AbstractValidator<CreateLessonCommand>
    {
        public CreateLessonCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(200).WithMessage("Name must not exceed 200 characters.");

            RuleFor(x => x.Content)
                .NotEmpty().WithMessage("Content is required.")
                .MaximumLength(50000).WithMessage("Content must not exceed 50000 characters.");

            /*RuleFor(x => x.CourseId)
                .NotEmpty().WithMessage("CourseId is required.");*/
        }
    }
}