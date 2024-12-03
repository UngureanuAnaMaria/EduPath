using Application.DTOs;
using MediatR;

namespace Application.Use_Cases.Queries
{
    public class GetLessonsQuery : IRequest<List<LessonDTO>>
    {
        public string? Name { get; set; }
        public Guid? CourseId { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }

}