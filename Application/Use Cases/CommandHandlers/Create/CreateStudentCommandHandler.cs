using Application.Use_Cases.Commands.Create;
using AutoMapper;
using Domain.Common;
using Domain.Entities;
using Domain.Repositories;
using MediatR;

namespace Application.Use_Cases.CommandHandlers
{
    public class CreateStudentCommandHandler : IRequestHandler<CreateStudentCommand, Result<Guid>>
    {
        private readonly IStudentRepository studentRepository;
        private readonly ICourseRepository courseRepository;
        private readonly IMapper mapper;

        public CreateStudentCommandHandler(IStudentRepository studentRepository, ICourseRepository courseRepository, IMapper mapper)
        {
            this.studentRepository = studentRepository;
            this.courseRepository = courseRepository;
            this.mapper = mapper;
        }

        public async Task<Result<Guid>> Handle(CreateStudentCommand request, CancellationToken cancellationToken)
        {
            var student = mapper.Map<Student>(request);

            var result = await studentRepository.AddStudentAsync(student);

            if (!result.IsSuccess)
            {
                return Result<Guid>.Failure(result.ErrorMessage);
            }

            return Result<Guid>.Success(student.Id);
        }
    }
}
