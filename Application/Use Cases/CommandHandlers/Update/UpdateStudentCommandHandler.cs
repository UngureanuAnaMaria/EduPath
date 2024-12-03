using Application.Use_Cases.Commands.Update;
using AutoMapper;
using Domain.Common;
using Domain.Entities;
using Domain.Repositories;
using MediatR;

namespace Application.Use_Cases.CommandHandlers.Update
{
    public class UpdateStudentCommandHandler : IRequestHandler<UpdateStudentCommand, Result<Guid>>
    {
        private readonly IStudentRepository studentRepository;
        private readonly IMapper mapper;

        public UpdateStudentCommandHandler(IStudentRepository studentRepository, IMapper mapper)
        {
            this.studentRepository = studentRepository;
            this.mapper = mapper;
        }

        public async Task<Result<Guid>> Handle(UpdateStudentCommand request, CancellationToken cancellationToken)
        {
            var student = mapper.Map<Student>(request);

            if (student == null)
            {
                return Result<Guid>.Failure("Student not found.");
            }

            student.Name = request.Name;
            student.Email = request.Email;
            student.Password = request.Password;
            student.Status = request.Status;
            student.CreatedAt = request.CreatedAt;
            student.LastLogin = request.LastLogin;

            if (request.StudentCourses != null)
            {
                foreach (var studentCourseCommand in request.StudentCourses)
                {
                    var studentCourse = new StudentCourse
                    {
                        Id = Guid.NewGuid(),
                        StudentId = student.Id,
                        CourseId = studentCourseCommand.CourseId
                    };
                    student.StudentCourses.Add(studentCourse);
                }
            }

            var result = await studentRepository.UpdateStudentAsync(student);

            if (result.IsSuccess)
            {
                return Result<Guid>.Success(student.Id); ;
            }

            return Result<Guid>.Failure(result.ErrorMessage);
        }
    }
}