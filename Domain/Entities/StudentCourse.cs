namespace Domain.Entities
{
    public class StudentCourse
    {
        public Guid Id { get; set; }
        public Guid StudentId { get; set; }
        public Student? Student { get; set; }
        public Guid CourseId { get; set; }
        public Course? Course { get; set; }

    }
}
