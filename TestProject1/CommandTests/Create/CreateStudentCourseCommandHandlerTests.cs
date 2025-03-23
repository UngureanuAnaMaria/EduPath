using Application.Use_Cases.CommandHandlers.Create;
using Application.Use_Cases.Commands.Create;
using AutoMapper;
using Domain.Common;
using Domain.Entities;
using Domain.Repositories;
using FluentAssertions;
using NSubstitute;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace TestProject1.CommandTests.Create
{
    public class CreateStudentCourseCommandHandlerTests
    {
        private readonly IStudentCourseRepository _repository;
        private readonly IMapper _mapper;
        private readonly CreateStudentCourseCommandHandler _handler;

        public CreateStudentCourseCommandHandlerTests()
        {
            _repository = Substitute.For<IStudentCourseRepository>();

            // Setup AutoMapper
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CreateStudentCourseCommand, StudentCourse>();
            });
            _mapper = config.CreateMapper();

            _handler = new CreateStudentCourseCommandHandler(_repository, _mapper);
        }
        /*
        [Fact]
        public async Task Handle_Should_Return_Success_When_StudentCourse_Creation_Succeeds()
        {
            // Arrange
            var command = new CreateStudentCourseCommand
            {
                CourseId = Guid.NewGuid()
            };

            var studentCourse = new StudentCourse
            {
                Id = Guid.NewGuid(),
                CourseId = command.CourseId,
                StudentId = Guid.NewGuid() // Assuming StudentId is generated internally
            };

            _repository.AddStudentCourseAsync(Arg.Any<StudentCourse>())
                .Returns(Result<Guid>.Success(studentCourse.Id));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Data.Should().Be(studentCourse.Id);
            await _repository.Received(1).AddStudentCourseAsync(Arg.Any<StudentCourse>());
        }
        */
        [Fact]
        public async Task Handle_Should_Return_Failure_When_StudentCourse_Creation_Fails()
        {
            // Arrange
            var command = new CreateStudentCourseCommand
            {
                CourseId = Guid.NewGuid()
            };

            _repository.AddStudentCourseAsync(Arg.Any<StudentCourse>())
                .Returns(Result<Guid>.Failure("Failed to create student course"));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.ErrorMessage.Should().Be("Failed to create student course");
            await _repository.Received(1).AddStudentCourseAsync(Arg.Any<StudentCourse>());
        }
    }
}
