using FluentValidation;

namespace Application.Use_Cases.Commands.Update
{
    public class UpdateLessonCommandValidator : AbstractValidator<UpdateLessonCommand>
    {
        public UpdateLessonCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(200).WithMessage("Name must not exceed 200 characters.");

            RuleFor(x => x.Content)
                .NotEmpty().WithMessage("Content is required.")
                .MaximumLength(50000).WithMessage("Content must not exceed 50000 characters.");

            /*RuleFor(x => x.CourseId)
                .NotEmpty().WithMessage("CourseId is required.");*/

            RuleFor(x => x.Id)
                .NotEmpty().Must(BeAValidGuid).WithMessage("'Id' must be a valid Guid.");
        }
        private bool BeAValidGuid(Guid guid)
        {
            return Guid.TryParse(guid.ToString(), out _);
        }
    }
}
