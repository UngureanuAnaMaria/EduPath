using Application.Use_Cases.CommandHandlers.Delete;
using Application.Use_Cases.Commands.Delete;
using Domain.Repositories;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Application.Tests.CommandHandlers
{
    public class DeleteStudentByIdCommandHandlerTests
    {
        private readonly IStudentRepository _studentRepository;
        private readonly DeleteStudentByIdCommandHandler _handler;

        public DeleteStudentByIdCommandHandlerTests()
        {
            _studentRepository = Substitute.For<IStudentRepository>();
            _handler = new DeleteStudentByIdCommandHandler(_studentRepository);
        }

        [Fact]
        public async Task Handle_Should_Call_DeleteStudentAsync_With_Correct_Id()
        {
            // Arrange
            var studentId = Guid.NewGuid();
            var command = new DeleteStudentByIdCommand { Id = studentId };

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            await _studentRepository.Received(1).DeleteStudentAsync(studentId);
        }

        [Fact]
        public async Task Handle_Should_Throw_Exception_When_Repository_Throws_Exception()
        {
            // Arrange
            var studentId = Guid.NewGuid();
            var command = new DeleteStudentByIdCommand { Id = studentId };

            _studentRepository.DeleteStudentAsync(studentId)
                .Returns(Task.FromException(new Exception("Database error")));

            // Act
            Func<Task> action = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await action.Should().ThrowAsync<Exception>()
                .WithMessage("Database error");
            await _studentRepository.Received(1).DeleteStudentAsync(studentId);
        }
    }
}
