using Application.Use_Cases.CommandHandlers;
using Application.Use_Cases.Commands.Create;
using AutoMapper;
using Domain.Common;
using Domain.Entities;
using Domain.Repositories;
using FluentAssertions;
using NSubstitute;

namespace Application.Tests.CommandHandlers
{
    public class CreateStudentCommandHandlerTests
    {
        private readonly IStudentRepository _studentRepository;
        private readonly ICourseRepository _courseRepository;
        private readonly IMapper _mapper;
        private readonly CreateStudentCommandHandler _handler;

        public CreateStudentCommandHandlerTests()
        {
            _studentRepository = Substitute.For<IStudentRepository>();
            _courseRepository = Substitute.For<ICourseRepository>();

            // Setup AutoMapper
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CreateStudentCommand, Student>();
                cfg.CreateMap<CreateStudentCourseCommand, StudentCourse>();
            });
            _mapper = config.CreateMapper();

            _handler = new CreateStudentCommandHandler(_studentRepository, _courseRepository, _mapper);
        }

        [Fact]
        public async Task Handle_Should_Return_Failure_Result_When_Repository_Fails()
        {
            // Arrange
            var command = new CreateStudentCommand
            {
                Name = "Jane Doe",
                Email = "jane.doe@example.com",
                Password = "SecurePass123",
                Status = false,
                CreatedAt = DateTime.UtcNow,
                LastLogin = null,
                StudentCourses = new List<CreateStudentCourseCommand>()
            };

            _studentRepository.AddStudentAsync(Arg.Any<Student>())
                              .Returns(Result<Guid>.Failure("Database error"));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.ErrorMessage.Should().Be("Database error");
            await _studentRepository.Received(1).AddStudentAsync(Arg.Any<Student>());
        }

        [Fact]
        public async Task Handle_Should_Map_Command_To_Student_Entity_Correctly()
        {
            // Arrange
            var command = new CreateStudentCommand
            {
                Name = "Alice Smith",
                Email = "alice.smith@example.com",
                Password = "MySecretPassword",
                Status = true,
                CreatedAt = DateTime.UtcNow,
                LastLogin = DateTime.UtcNow.AddHours(-2),
                StudentCourses = new List<CreateStudentCourseCommand>
                {
                    new CreateStudentCourseCommand { CourseId = Guid.NewGuid() }
                }
            };

            var capturedStudent = new Student();

            _studentRepository.AddStudentAsync(Arg.Do<Student>(x => capturedStudent = x))
                              .Returns(Result<Guid>.Success(Guid.NewGuid()));

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            capturedStudent.Name.Should().Be(command.Name);
            capturedStudent.Email.Should().Be(command.Email);
            capturedStudent.Password.Should().Be(command.Password);
            capturedStudent.Status.Should().Be(command.Status);
            capturedStudent.CreatedAt.Should().Be(command.CreatedAt);
            capturedStudent.LastLogin.Should().Be(command.LastLogin);
            capturedStudent.StudentCourses.Should().NotBeNull();
            capturedStudent.StudentCourses!.Count.Should().Be(1);
        }

    }
}

    
