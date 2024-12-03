namespace Domain.Entities
{
    public class Course
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description {  get; set; }
        public Guid? ProfessorId { get; set; }
        public Professor? Professor { get; set; }
        public List<Lesson>? Lessons { get; set; }
      
        public List<StudentCourse>? StudentCourses { get; set; }

    }
}
