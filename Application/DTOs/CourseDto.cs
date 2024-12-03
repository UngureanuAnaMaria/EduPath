using Domain.Entities;

namespace Application.DTOs
{
    public class CourseDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid? ProfessorId { get; set; }
        public ProfessorDTO? Professor { get; set; }
        public List<LessonDTO>? Lessons { get; set; }
        public List<StudentCourseDTO>? StudentCourses { get; set; }
    }
}
