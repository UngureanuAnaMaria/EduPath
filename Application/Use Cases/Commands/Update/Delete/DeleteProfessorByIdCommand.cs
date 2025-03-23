using MediatR;

namespace Application.Use_Cases.Commands.Delete
{
    public class DeleteProfessorByIdCommand : IRequest
    {
        public Guid Id { get; set; }
    }
}