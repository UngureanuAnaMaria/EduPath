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

namespace Application.Tests.CommandHandlers.Create
{
    public class CreateCourseCommandHandlerTests
    {
        private readonly ICourseRepository _courseRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly IProfessorRepository _professorRepository;
        private readonly IMapper _mapper;
        private readonly CreateCourseCommandHandler _handler;

        public CreateCourseCommandHandlerTests()
        {
            _courseRepository = Substitute.For<ICourseRepository>();
            _studentRepository = Substitute.For<IStudentRepository>();
            _professorRepository = Substitute.For<IProfessorRepository>();

            // Setup AutoMapper
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CreateCourseCommand, Course>();
            });
            _mapper = config.CreateMapper();

            _handler = new CreateCourseCommandHandler(_courseRepository, _studentRepository, _mapper, _professorRepository);
        }

        [Fact]
        public async Task Handle_Should_Return_Failure_When_Course_Creation_Fails()
        {
            // Arrange
            var command = new CreateCourseCommand
            {
                Name = "Test Course",
                Description = "Course Description",
                ProfessorId = Guid.NewGuid()
            };

            _courseRepository.AddCourseAsync(Arg.Any<Course>())
                .Returns(Result<Guid>.Failure("Failed to create course"));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.ErrorMessage.Should().Be("Failed to create course");
            await _courseRepository.Received(1).AddCourseAsync(Arg.Any<Course>());
        }
    }
}
