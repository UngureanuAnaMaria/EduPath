using MediatR;

namespace Application.Use_Cases.Commands.Delete
{
    public class DeleteStudentByIdCommand : IRequest
    {
        public Guid Id { get; set; }
    }
}