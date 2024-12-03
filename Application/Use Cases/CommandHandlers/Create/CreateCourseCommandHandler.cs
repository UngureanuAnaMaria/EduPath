using Domain.Common;
using Application.Use_Cases.Commands.Create;
using AutoMapper;
using Domain.Entities;
using Domain.Repositories;
using MediatR;

namespace Application.Use_Cases.CommandHandlers.Create
{
    public class CreateCourseCommandHandler : IRequestHandler<CreateCourseCommand, Result<Guid>>
    {
        private readonly ICourseRepository courseRepository;
        private readonly IStudentRepository studentRepository;
        private readonly IProfessorRepository professorRepository;
        private readonly IMapper mapper;

        public CreateCourseCommandHandler(ICourseRepository courseRepository, IStudentRepository studentRepository, IMapper mapper, IProfessorRepository professorRepository)
        {
            this.courseRepository = courseRepository;
            this.studentRepository = studentRepository;
            this.mapper = mapper;
            this.professorRepository = professorRepository;
        }

        public async Task<Result<Guid>> Handle(CreateCourseCommand request, CancellationToken cancellationToken)
        {
            var course = mapper.Map<Course>(request);

            var result = await courseRepository.AddCourseAsync(course);

            if (!result.IsSuccess)
            {
                return Result<Guid>.Failure(result.ErrorMessage);
            }

            return Result<Guid>.Success(course.Id);
        }


    }
}
