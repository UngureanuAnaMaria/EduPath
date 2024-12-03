namespace Domain.Entities
{
    public class Lesson
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
        public Guid CourseId { get; set; }
        public Course Course { get; set; }
    }
}
