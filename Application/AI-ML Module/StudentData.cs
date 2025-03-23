using Application.DTOs;

namespace Application.AI_ML_Module
{
    public class StudentData
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastLogin { get; set; }
        //public List<string> StudentCourses { get; set; }
        public float AverageGrade { get; set; }
        public float PercentageCompletedCourses { get; set; }
        public string LearningPath { get; set; }
    }

}
