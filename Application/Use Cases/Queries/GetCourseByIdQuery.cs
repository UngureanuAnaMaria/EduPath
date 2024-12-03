using Application.DTOs;
using MediatR;

namespace Application.Use_Cases.Queries
{
    public class GetCourseByIdQuery : IRequest<CourseDTO>
    {
        public Guid Id { get; set; }
    }
}