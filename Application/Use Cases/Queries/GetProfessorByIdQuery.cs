using Application.DTOs;
using MediatR;

namespace Application.Use_Cases.Queries
{
    public class GetProfessorByIdQuery : IRequest<ProfessorDTO>
    {
        public Guid Id { get; set; }
    }
}