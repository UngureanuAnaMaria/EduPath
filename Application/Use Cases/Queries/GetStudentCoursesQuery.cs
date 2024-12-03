using Application.DTOs;
using MediatR;

namespace Application.Use_Cases.Queries
{
    public class GetStudentCoursesQuery : IRequest<List<StudentCourseDTO>>
    {
        public Guid? StudentId { get; set; }
        public Guid? CourseId { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }

}
