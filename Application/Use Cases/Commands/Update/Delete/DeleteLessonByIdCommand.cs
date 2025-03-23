using MediatR;

namespace Application.Use_Cases.Commands.Delete
{
    public class DeleteLessonByIdCommand : IRequest
    {
        public Guid Id { get; set; }
    }
}