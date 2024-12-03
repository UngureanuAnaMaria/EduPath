using Application.Use_Cases.Commands.Delete;
using Domain.Repositories;
using MediatR;

namespace Application.Use_Cases.CommandHandlers.Delete
{
    public class DeleteStudentByIdCommandHandler : IRequestHandler<DeleteStudentByIdCommand>
    {
        private readonly IStudentRepository studentRepository;

        public DeleteStudentByIdCommandHandler(IStudentRepository studentRepository)
        {
            this.studentRepository = studentRepository;
        }

        public async Task Handle(DeleteStudentByIdCommand request, CancellationToken cancellationToken)
        {
            await studentRepository.DeleteStudentAsync(request.Id);
        }
    }
}