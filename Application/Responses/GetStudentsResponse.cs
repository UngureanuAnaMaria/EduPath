using Application.DTOs;

namespace Application.Responses
{
    public class GetStudentsResponse
    {
        public List<StudentDTO> Students { get; set; }
        public int TotalCount { get; set; }
    }
}
