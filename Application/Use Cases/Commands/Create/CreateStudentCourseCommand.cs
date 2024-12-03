using Domain.Common;
using Domain.Entities;
using MediatR;

namespace Application.Use_Cases.Commands.Create
{
    public class CreateStudentCourseCommand : IRequest<Result<Guid>>
    {
        public Guid CourseId { get; set; }
        //public Course? Course { get; set; }
    }
}
