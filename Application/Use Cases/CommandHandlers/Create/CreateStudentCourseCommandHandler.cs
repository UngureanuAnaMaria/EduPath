using Application.Use_Cases.Commands.Create;
using AutoMapper;
using Domain.Common;
using Domain.Entities;
using Domain.Repositories;
using MediatR;

namespace Application.Use_Cases.CommandHandlers.Create
{
    public class CreateStudentCourseCommandHandler : IRequestHandler<CreateStudentCourseCommand, Result<Guid>>
    {
        private readonly IStudentCourseRepository repository;
        private readonly IMapper mapper;

        public CreateStudentCourseCommandHandler(IStudentCourseRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public async Task<Result<Guid>> Handle(CreateStudentCourseCommand request, CancellationToken cancellationToken)
        {
            var studentCourse = mapper.Map<StudentCourse>(request);

            var result = await repository.AddStudentCourseAsync(studentCourse);

            if (!result.IsSuccess)
            {
                return Result<Guid>.Failure(result.ErrorMessage);
            }

            return Result<Guid>.Success(studentCourse.Id);
        }
    }
}
