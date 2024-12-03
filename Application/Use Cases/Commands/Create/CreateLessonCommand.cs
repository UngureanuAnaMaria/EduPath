using Domain.Common;
using MediatR;

namespace Application.Use_Cases.Commands.Create
{
    public class CreateLessonCommand : IRequest<Result<Guid>>
    {
        public required string Name { get; set; }
        public required string Content { get; set; }
        public Guid CourseId { get; set; }
    }
}