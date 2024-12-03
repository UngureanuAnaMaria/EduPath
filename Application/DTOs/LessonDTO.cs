using Domain.Entities;

namespace Application.DTOs
{
    public class LessonDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
        public Guid CourseId { get; set; }
        public CourseDTO Course { get; set; }
    }
}
