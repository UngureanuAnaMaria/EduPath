using Application.DTOs;
using MediatR;

namespace Application.Use_Cases.Queries
{
    public class GetStudentByIdQuery : IRequest<StudentDTO>
    {
        public Guid Id { get; set; }
    }
}