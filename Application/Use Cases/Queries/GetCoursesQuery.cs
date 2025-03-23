using Application.DTOs;
using Application.Responses;
using MediatR;

namespace Application.Use_Cases.Queries
{
    public class GetCoursesQuery : IRequest<GetCoursesResponse>
    {
        public string? Name { get; set; }
        public Guid? ProfessorId { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }

}
