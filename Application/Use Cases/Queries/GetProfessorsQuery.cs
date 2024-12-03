using Application.DTOs;
using MediatR;

namespace Application.Use_Cases.Queries
{
    public class GetProfessorsQuery : IRequest<List<ProfessorDTO>>
    {
        public string? Name { get; set; }
        public bool? Status { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}