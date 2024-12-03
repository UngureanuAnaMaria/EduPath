using Application.Use_Cases.Commands.Update;
using AutoMapper;
using Domain.Common;
using Domain.Entities;
using Domain.Repositories;
using MediatR;

namespace Application.Use_Cases.CommandHandlers.Update
{
    public class UpdateLessonCommandHandler : IRequestHandler<UpdateLessonCommand, Result<Guid>>
    {
        private readonly ILessonRepository repository;
        private readonly IMapper mapper;

        public UpdateLessonCommandHandler(ILessonRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public async Task<Result<Guid>> Handle(UpdateLessonCommand request, CancellationToken cancellationToken)
        {
            var lesson = mapper.Map<Lesson>(request);

            var result = await repository.UpdateLessonAsync(lesson);
            if (result.IsSuccess)
            {
                return Result<Guid>.Success(lesson.Id);
            }
            return Result<Guid>.Failure(result.ErrorMessage);
        }
    }
}