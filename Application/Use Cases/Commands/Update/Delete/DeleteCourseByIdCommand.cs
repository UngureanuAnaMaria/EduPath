using MediatR;

namespace Application.Use_Cases.Commands.Delete
{
    public class DeleteCourseByIdCommand : IRequest
    {
        public Guid Id { get; set; }
    }
}