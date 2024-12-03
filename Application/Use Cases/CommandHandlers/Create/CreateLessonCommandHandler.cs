using Domain.Common;
using Application.Use_Cases.Commands.Create;
using AutoMapper;
using Domain.Entities;
using Domain.Repositories;
using MediatR;

namespace Application.Use_Cases.CommandHandlers.Create
{
    public class CreateLessonCommandHandler : IRequestHandler<CreateLessonCommand, Result<Guid>>
    {
        private readonly ILessonRepository repository;
        private readonly IMapper mapper;

        public CreateLessonCommandHandler(ILessonRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }
        public async Task<Result<Guid>> Handle(CreateLessonCommand request, CancellationToken cancellationToken)
        {
            var lesson = mapper.Map<Lesson>(request);
            
            var result = await repository.AddLessonAsync(lesson);
            if (result.IsSuccess)
            {
                return Result<Guid>.Success(result.Data);
            }
            return Result<Guid>.Failure(result.ErrorMessage);
        }
    }
}