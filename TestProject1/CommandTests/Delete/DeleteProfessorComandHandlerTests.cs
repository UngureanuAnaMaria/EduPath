using Application.Use_Cases.CommandHandlers.Delete;
using Application.Use_Cases.Commands.Delete;
using Domain.Repositories;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Xunit;

namespace Application.Tests.CommandHandlers
{
    public class DeleteProfessorCommandHandlerTests
    {
        private readonly IProfessorRepository _professorRepository;
        private readonly DeleteProfessorByIdCommandHandler _handler;

        public DeleteProfessorCommandHandlerTests()
        {
            _professorRepository = Substitute.For<IProfessorRepository>();
            _handler = new DeleteProfessorByIdCommandHandler(_professorRepository);
        }

        [Fact]
        public async Task Handle_Should_Call_Repository_DeleteProfessorAsync_With_Correct_Id()
        {
            // Arrange
            var professorId = Guid.NewGuid();
            var command = new DeleteProfessorByIdCommand { Id = professorId };

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            await _professorRepository.Received(1).DeleteProfessorAsync(professorId);
        }

        [Fact]
        public async Task Handle_Should_Not_Throw_Exception_When_Repository_Delete_Succeeds()
        {
            // Arrange
            var professorId = Guid.NewGuid();
            var command = new DeleteProfessorByIdCommand { Id = professorId };

            _professorRepository.DeleteProfessorAsync(professorId).Returns(Task.CompletedTask);

            // Act
            var exception = await Record.ExceptionAsync(() => _handler.Handle(command, CancellationToken.None));

            // Assert
            exception.Should().BeNull();
            await _professorRepository.Received(1).DeleteProfessorAsync(professorId);
        }

        [Fact]
        public async Task Handle_Should_Throw_Exception_When_Repository_Delete_Fails()
        {
            // Arrange
            var professorId = Guid.NewGuid();
            var command = new DeleteProfessorByIdCommand { Id = professorId };

            _professorRepository.DeleteProfessorAsync(professorId).Throws(new Exception("Delete failed"));

            // Act
            var exception = await Record.ExceptionAsync(() => _handler.Handle(command, CancellationToken.None));

            // Assert
            exception.Should().NotBeNull();
            exception.Message.Should().Be("Delete failed");
            await _professorRepository.Received(1).DeleteProfessorAsync(professorId);
        }
    }
}
