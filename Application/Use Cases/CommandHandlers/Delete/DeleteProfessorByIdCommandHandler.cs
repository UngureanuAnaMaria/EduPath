using Application.Use_Cases.Commands.Delete;
using Domain.Repositories;
using MediatR;

namespace Application.Use_Cases.CommandHandlers.Delete
{
    public class DeleteProfessorByIdCommandHandler : IRequestHandler<DeleteProfessorByIdCommand>
    {
        private readonly IProfessorRepository repository;

        public DeleteProfessorByIdCommandHandler(IProfessorRepository repository)
        {
            this.repository = repository;
        }

        public async Task Handle(DeleteProfessorByIdCommand request, CancellationToken cancellationToken)
        {
            await repository.DeleteProfessorAsync(request.Id);
        }
    }
}
