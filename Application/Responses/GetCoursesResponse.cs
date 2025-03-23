using Application.DTOs;

namespace Application.Responses
{
    public class GetCoursesResponse
    {
        public List<CourseDTO> Courses { get; set; }
        public int TotalCount { get; set; }
    }
}
