using Application.Use_Cases.Commands.Delete;
using Domain.Repositories;
using MediatR;

namespace Application.Use_Cases.CommandHandlers.Delete
{
    public class DeleteCourseCommandHandler : IRequestHandler<DeleteCourseByIdCommand>
    {
        private readonly ICourseRepository courseRepository;

        public DeleteCourseCommandHandler(ICourseRepository courseRepository)
        {
            this.courseRepository = courseRepository;
        }

        public async Task Handle(DeleteCourseByIdCommand request, CancellationToken cancellationToken)
        {
            await courseRepository.DeleteCourseAsync(request.Id);
        }
    }
}