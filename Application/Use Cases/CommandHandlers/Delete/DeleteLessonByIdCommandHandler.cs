using Application.Use_Cases.Commands.Delete;
using Domain.Repositories;
using MediatR;

namespace Application.Use_Cases.CommandHandlers.Delete
{
    public class DeleteLessonByIdCommandHandler : IRequestHandler<DeleteLessonByIdCommand>
    {
        private readonly ILessonRepository repository;

        public DeleteLessonByIdCommandHandler(ILessonRepository repository)
        {
            this.repository = repository;
        }

        public async Task Handle(DeleteLessonByIdCommand request, CancellationToken cancellationToken)
        {
            await repository.DeleteLessonAsync(request.Id);
        }
    }
}