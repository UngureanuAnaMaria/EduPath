using System;
using System.Security.Policy;
using System.Threading;
using System.Threading.Tasks;
using Application.Use_Cases.CommandHandlers.Delete;
using Application.Use_Cases.Commands.Delete; 
using Domain.Repositories;
using MediatR;
using NSubstitute;
using Xunit;

namespace Application.Tests.CommandHandlers.Delete
{
    public class DeleteCourseCommandHandlerTests
    {
        private readonly ICourseRepository _courseRepository;
        private readonly DeleteCourseCommandHandler _handler;

        public DeleteCourseCommandHandlerTests()
        {
            _courseRepository = Substitute.For<ICourseRepository>();
            _handler = new DeleteCourseCommandHandler(_courseRepository);
        }

        [Fact]
        public async Task Handle_Should_Delete_Course_Successfully()
        {
            // Arrange
            var courseId = Guid.NewGuid();  // Create a valid Guid for the course
            var command = new DeleteCourseByIdCommand { Id = courseId };

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            await _courseRepository.Received(1).DeleteCourseAsync(courseId);  // Assert that DeleteCourseAsync was called with the correct courseId
        }
    }
}
