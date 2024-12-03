using Domain.Common;
using Domain.Entities;
using MediatR;

namespace Application.Use_Cases.Commands.Create
{
    public class CreateCourseStudentCommand : IRequest<Result<Guid>>
    {
        public Guid StudentId { get; set; }
        //public Student? Student { get; set; }
    }
}
