using Application.Use_Cases.CommandHandlers.Create;
using Application.Use_Cases.Commands.Create;
using AutoMapper;
using Domain.Common;
using Domain.Entities;
using Domain.Repositories;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Application.Tests.CommandHandlers
{
    public class CreateProfessorCommandHandlerTests
    {
        private readonly IProfessorRepository _professorRepository;
        private readonly IMapper _mapper;
        private readonly CreateProfessorCommandHandler _handler;

        public CreateProfessorCommandHandlerTests()
        {
            _professorRepository = Substitute.For<IProfessorRepository>();

            // Setup AutoMapper
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CreateProfessorCommand, Professor>();
            });
            _mapper = config.CreateMapper();

            _handler = new CreateProfessorCommandHandler(_professorRepository, _mapper);
        }

     
        [Fact]
        public async Task Handle_Should_Map_Command_To_Professor_Entity_Correctly()
        {
            // Arrange
            var command = new CreateProfessorCommand
            {
                Name = "Dr. Alice Smith",
                Email = "alice.smith@example.com",
                Password = "SecretPassword123",
                Status = true,
                CreatedAt = DateTime.UtcNow
            };

            var capturedProfessor = new Professor();

            _professorRepository.AddProfessorAsync(Arg.Do<Professor>(x => capturedProfessor = x))
                                .Returns(Result<Guid>.Success(Guid.NewGuid()));

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            capturedProfessor.Name.Should().Be(command.Name);
            capturedProfessor.Email.Should().Be(command.Email);
            capturedProfessor.Password.Should().Be(command.Password);
            capturedProfessor.Status.Should().Be(command.Status);
            capturedProfessor.CreatedAt.Should().Be(command.CreatedAt);
        }
    }
}
