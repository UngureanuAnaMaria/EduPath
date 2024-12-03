using Domain.Common;
using MediatR;

namespace Application.Use_Cases.Commands.Create
{
    public class CreateStudentCommand : IRequest<Result<Guid>>
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? LastLogin { get; set; }
        public List<CreateStudentCourseCommand>? StudentCourses { get; set; }
    }
}