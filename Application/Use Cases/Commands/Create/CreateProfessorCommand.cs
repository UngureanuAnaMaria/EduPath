using Domain.Common;
using Domain.Entities;
using MediatR;

namespace Application.Use_Cases.Commands.Create
{
    public class CreateProfessorCommand : IRequest<Result<Guid>>
    {
        public required string Name { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public bool Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? LastLogin { get; set; }
        //public List<Course>? Courses { get; set; }
    }
}