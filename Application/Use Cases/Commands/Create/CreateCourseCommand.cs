using Domain.Common;
using Domain.Entities;
using MediatR;

namespace Application.Use_Cases.Commands.Create
{
    public class CreateCourseCommand : IRequest<Result<Guid>>
    {
        public required string Name { get; set; }
        public required string Description { get; set; }
        public Guid? ProfessorId { get; set; }
        //public Professor? Professor { get; set; }
        //public List<Lesson>? Lessons { get; set; } 
        public List<CreateCourseStudentCommand>? StudentCourses { get; set; }

    }
}