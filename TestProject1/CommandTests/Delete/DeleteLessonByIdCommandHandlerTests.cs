using Application.Use_Cases.CommandHandlers.Delete;
using Application.Use_Cases.Commands.Delete;
using Domain.Repositories;
using NSubstitute;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace TestProject1.CommandTests.Delete
{
    public class DeleteLessonByIdCommandHandlerTests
    {
        private readonly ILessonRepository _repository;
        private readonly DeleteLessonByIdCommandHandler _handler;

        public DeleteLessonByIdCommandHandlerTests()
        {
            _repository = Substitute.For<ILessonRepository>();
            _handler = new DeleteLessonByIdCommandHandler(_repository);
        }

        [Fact]
        public async Task Handle_Should_Call_DeleteLessonAsync()
        {
            // Arrange
            var command = new DeleteLessonByIdCommand
            {
                Id = Guid.NewGuid()
            };

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            await _repository.Received(1).DeleteLessonAsync(command.Id);
        }
    }
}
