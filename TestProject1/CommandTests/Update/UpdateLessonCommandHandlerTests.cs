using Application.Use_Cases.CommandHandlers.Update;
using Application.Use_Cases.Commands.Update;
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

namespace TestProject1.CommandTests.Update
{
    public class UpdateLessonCommandHandlerTests
    {
        private readonly ILessonRepository _repository;
        private readonly IMapper _mapper;
        private readonly UpdateLessonCommandHandler _handler;

        public UpdateLessonCommandHandlerTests()
        {
            _repository = Substitute.For<ILessonRepository>();

            // Setup AutoMapper
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<UpdateLessonCommand, Lesson>();
            });
            _mapper = config.CreateMapper();

            _handler = new UpdateLessonCommandHandler(_repository, _mapper);
        }
        /*
        [Fact]
        public async Task Handle_Should_Return_Success_When_Lesson_Update_Succeeds()
        {
            // Arrange
            var command = new UpdateLessonCommand
            {
                Id = Guid.NewGuid(),
                Name = "Updated Lesson",
                Content = "Updated Content",
                CourseId = Guid.NewGuid()
            };

            var lesson = new Lesson
            {
                Id = command.Id,
                Name = command.Name,
                Content = command.Content,
                CourseId = command.CourseId
            };

            _repository.UpdateLessonAsync(Arg.Any<Lesson>())
                .Returns(Result<Guid>.Success(lesson.Id));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Data.Should().Be(lesson.Id);
            await _repository.Received(1).UpdateLessonAsync(Arg.Any<Lesson>());
        }
        */
        [Fact]
        public async Task Handle_Should_Return_Failure_When_Lesson_Update_Fails()
        {
            // Arrange
            var command = new UpdateLessonCommand
            {
                Id = Guid.NewGuid(),
                Name = "Updated Lesson",
                Content = "Updated Content",
                CourseId = Guid.NewGuid()
            };

            _repository.UpdateLessonAsync(Arg.Any<Lesson>())
                .Returns(Result<Guid>.Failure("Failed to update lesson"));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.ErrorMessage.Should().Be("Failed to update lesson");
            await _repository.Received(1).UpdateLessonAsync(Arg.Any<Lesson>());
        }
    }
}
