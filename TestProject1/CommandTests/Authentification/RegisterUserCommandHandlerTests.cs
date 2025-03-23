using Application.Use_Cases.Authentification;
using Domain.Entities;
using Domain.Repositories;
using FluentAssertions;
using NSubstitute;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace TestProject1.CommandTests.Authentification
{
    public class RegisterUserCommandHandlerTests
    {
        private readonly IUserRepository _userRepository;
        private readonly RegisterUserCommandHandler _handler;

        public RegisterUserCommandHandlerTests()
        {
            _userRepository = Substitute.For<IUserRepository>();
            _handler = new RegisterUserCommandHandler(_userRepository);
        }

        [Fact]
        public async Task Handle_Should_Return_UserId_When_Registration_Is_Successful()
        {
            // Arrange
            var command = new RegisterUserCommand
            {
                Email = "test@example.com",
                Password = "Password123",
                Admin = true
            };

            var userId = Guid.NewGuid();
            _userRepository.Register(Arg.Do<User>(user => user.Id = userId), Arg.Any<CancellationToken>())
                .Returns(Task.FromResult(userId));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().Be(userId);
            await _userRepository.Received(1).Register(Arg.Any<User>(), Arg.Any<CancellationToken>());
        }




        [Fact]
        public async Task Handle_Should_Throw_Exception_When_Registration_Fails()
        {
            // Arrange
            var command = new RegisterUserCommand
            {
                Email = "test@example.com",
                Password = "Password123",
                Admin = true
            };

            _userRepository.Register(Arg.Any<User>(), Arg.Any<CancellationToken>())
                .Returns<Task<Guid>>(x => throw new Exception("Registration failed"));

            // Act
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<Exception>().WithMessage("Registration failed");
            await _userRepository.Received(1).Register(Arg.Any<User>(), Arg.Any<CancellationToken>());
        }

    }
}
