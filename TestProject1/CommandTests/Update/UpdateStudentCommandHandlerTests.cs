using Application.Use_Cases.CommandHandlers.Update;
using Application.Use_Cases.Commands.Update;
using AutoMapper;
using Domain.Common;
using Domain.Entities;
using Domain.Repositories;
using FluentAssertions;
using MediatR;
using NSubstitute;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Application.Tests.CommandHandlers.Update
{
    public class UpdateStudentCommandHandlerTests
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IMapper _mapper;
        private readonly UpdateStudentCommandHandler _handler;

        public UpdateStudentCommandHandlerTests()
        {
            _studentRepository = Substitute.For<IStudentRepository>();

            // Setup AutoMapper
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<UpdateStudentCommand, Student>();
            });
            _mapper = config.CreateMapper();

            _handler = new UpdateStudentCommandHandler(_studentRepository, _mapper);
        }

        [Fact]
        public async Task Handle_Should_Return_Failure_When_Student_Not_Found()
        {
            // Arrange
            var command = new UpdateStudentCommand
            {
                Id = Guid.NewGuid(),
                Name = "Jane Doe",
                Email = "jane.doe@example.com",
                Password = "NewPassword123",
                Status = true,
                CreatedAt = DateTime.UtcNow,
                LastLogin = DateTime.UtcNow
            };

            _studentRepository.UpdateStudentAsync(Arg.Any<Student>())
                .Returns(Result<Guid>.Failure("Student not found"));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.ErrorMessage.Should().Be("Student not found");
            await _studentRepository.Received(1).UpdateStudentAsync(Arg.Any<Student>());
        }

        [Fact]
        public async Task Handle_Should_Return_Failure_When_Update_Fails()
        {
            // Arrange
            var studentId = Guid.NewGuid();
            var command = new UpdateStudentCommand
            {
                Id = studentId,
                Name = "Mark Twain",
                Email = "mark.twain@example.com",
                Password = "AnotherPassword",
                Status = true,
                CreatedAt = DateTime.UtcNow,
                LastLogin = DateTime.UtcNow
            };

            _studentRepository.UpdateStudentAsync(Arg.Any<Student>())
                .Returns(Result<Guid>.Failure("Failed to update"));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.ErrorMessage.Should().Be("Failed to update");
            await _studentRepository.Received(1).UpdateStudentAsync(Arg.Any<Student>());
        }
    }
}
