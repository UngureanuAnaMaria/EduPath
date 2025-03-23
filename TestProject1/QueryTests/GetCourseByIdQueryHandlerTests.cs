using Application.Use_Cases.QueryHandlers;
using Application.Use_Cases.Queries;
using AutoMapper;
using Domain.Entities;
using Domain.Repositories;
using FluentAssertions;
using NSubstitute;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Application.DTOs;

namespace TestProject1.QueryTests
{
    public class GetCourseByIdQueryHandlerTests
    {
        private readonly ICourseRepository _courseRepository;
        private readonly IProfessorRepository _professorRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly ILessonRepository _lessonRepository;
        private readonly IStudentCourseRepository _studentCourseRepository;
        private readonly IMapper _mapper;
        private readonly GetCourseByIdQueryHandler _handler;

        public GetCourseByIdQueryHandlerTests()
        {
            _courseRepository = Substitute.For<ICourseRepository>();
            _professorRepository = Substitute.For<IProfessorRepository>();
            _studentRepository = Substitute.For<IStudentRepository>();
            _lessonRepository = Substitute.For<ILessonRepository>();
            _studentCourseRepository = Substitute.For<IStudentCourseRepository>();

            // Setup AutoMapper
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Course, CourseDTO>();
            });
            _mapper = config.CreateMapper();

            _handler = new GetCourseByIdQueryHandler(_courseRepository, _professorRepository, _studentRepository, _lessonRepository, _studentCourseRepository, _mapper);
        }

        [Fact]
        public async Task Handle_Should_Return_Course_When_Course_Exists()
        {
            // Arrange
            var courseId = Guid.NewGuid();
            var course = new Course
            {
                Id = courseId,
                Name = "Test Course",
                Description = "Course Description"
            };

            _courseRepository.GetCourseByIdAsync(courseId)
                .Returns(Task.FromResult<Course?>(course));

            var query = new GetCourseByIdQuery { Id = courseId };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(courseId);
            result.Name.Should().Be("Test Course");
            result.Description.Should().Be("Course Description");
            await _courseRepository.Received(1).GetCourseByIdAsync(courseId);
        }
        /*
        [Fact]
        public async Task Handle_Should_Return_Null_When_Course_Does_Not_Exist()
        {
            // Arrange
            var courseId = Guid.NewGuid();

            _courseRepository.GetCourseByIdAsync(courseId)
                .Returns(Task.FromResult<Course?>(null));

            var query = new GetCourseByIdQuery { Id = courseId };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().BeNull();
            await _courseRepository.Received(1).GetCourseByIdAsync(courseId);
        }*/
    }
}
