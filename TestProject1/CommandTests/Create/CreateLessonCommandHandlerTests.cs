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
    public class CreateLessonCommandHandlerTests
    {
        private readonly ILessonRepository _repository;
        private readonly IMapper _mapper;
        private readonly CreateLessonCommandHandler _handler;

        public CreateLessonCommandHandlerTests()
        {
            _repository = Substitute.For<ILessonRepository>();

            // Setup AutoMapper
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CreateLessonCommand, Lesson>();
            });
            _mapper = config.CreateMapper();

            _handler = new CreateLessonCommandHandler(_repository, _mapper);
        }

        [Fact]
        public async Task Handle_Should_Return_Success_When_Lesson_Creation_Succeeds()
        {
            // Arrange
            var command = new CreateLessonCommand
            {
                Name = "Test Lesson",
                Content = "Test Content",
                CourseId = Guid.NewGuid()
            };

            var lesson = new Lesson
            {
                Id = Guid.NewGuid(),
                Name = command.Name,
                Content = command.Content,
                CourseId = command.CourseId
            };

            _repository.AddLessonAsync(Arg.Any<Lesson>())
                .Returns(Result<Guid>.Success(lesson.Id));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Data.Should().Be(lesson.Id);
            await _repository.Received(1).AddLessonAsync(Arg.Any<Lesson>());
        }

        [Fact]
        public async Task Handle_Should_Return_Failure_When_Lesson_Creation_Fails()
        {
            // Arrange
            var command = new CreateLessonCommand
            {
                Name = "Test Lesson",
                Content = "Test Content",
                CourseId = Guid.NewGuid()
            };

            _repository.AddLessonAsync(Arg.Any<Lesson>())
                .Returns(Result<Guid>.Failure("Failed to create lesson"));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.ErrorMessage.Should().Be("Failed to create lesson");
            await _repository.Received(1).AddLessonAsync(Arg.Any<Lesson>());
        }
    }
}
