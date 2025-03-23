using Application.Use_Cases.QueryHandlers;
using Application.Use_Cases.Queries;
using AutoMapper;
using Domain.Entities;
using Domain.Repositories;
using FluentAssertions;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Application.DTOs;

namespace TestProject1.QueryTests
{
    public class GetCoursesQueryHandlerTests
    {
        private readonly ICourseRepository _courseRepository;
        private readonly IStudentCourseRepository _studentCourseRepository;
        private readonly ILessonRepository _lessonRepository;
        private readonly IProfessorRepository _professorRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly IMapper _mapper;
        private readonly GetCoursesQueryHandler _handler;

        public GetCoursesQueryHandlerTests()
        {
            _courseRepository = Substitute.For<ICourseRepository>();
            _studentCourseRepository = Substitute.For<IStudentCourseRepository>();
            _lessonRepository = Substitute.For<ILessonRepository>();
            _professorRepository = Substitute.For<IProfessorRepository>();
            _studentRepository = Substitute.For<IStudentRepository>();

            // Setup AutoMapper
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Course, CourseDTO>();
            });
            _mapper = config.CreateMapper();

            _handler = new GetCoursesQueryHandler(_courseRepository, _studentCourseRepository, _lessonRepository, _professorRepository, _studentRepository, _mapper);
        }
        /*
        [Fact]
        public async Task Handle_Should_Return_Courses_When_Courses_Exist()
        {
            // Arrange
            var courses = new List<Course>
            {
                new Course { Id = Guid.NewGuid(), Name = "Course 1", Description = "Description 1" },
                new Course { Id = Guid.NewGuid(), Name = "Course 2", Description = "Description 2" }
            };

            _courseRepository.GetAllCoursesAsync().Returns(Task.FromResult<IEnumerable<Course>>(courses));

            var query = new GetCoursesQuery();

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(2);
            result.Should().Contain(c => c.Name == "Course 1");
            result.Should().Contain(c => c.Name == "Course 2");
            await _courseRepository.Received(1).GetAllCoursesAsync();
        }

        [Fact]
        public async Task Handle_Should_Return_Empty_List_When_No_Courses_Exist()
        {
            // Arrange
            _courseRepository.GetAllCoursesAsync().Returns(Task.FromResult<IEnumerable<Course>>(new List<Course>()));

            var query = new GetCoursesQuery();

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEmpty();
            await _courseRepository.Received(1).GetAllCoursesAsync();
        }
        */
        [Fact]
        public async Task Handle_Should_Return_Filtered_Courses_When_Query_Has_Filters()
        {
            // Arrange
            var courses = new List<Course>
            {
                new Course { Id = Guid.NewGuid(), Name = "Course 1", Description = "Description 1", ProfessorId = Guid.NewGuid() },
                new Course { Id = Guid.NewGuid(), Name = "Course 2", Description = "Description 2", ProfessorId = Guid.NewGuid() }
            };

            var filteredCourses = new List<Course> { courses[0] };

            _courseRepository.GetFilteredCoursesAsync("Course 1", null, 1, 10)
                .Returns(Task.FromResult<(IEnumerable<Course>, int)>((filteredCourses, 1)));

            var query = new GetCoursesQuery { Name = "Course 1", PageNumber = 1, PageSize = 10 };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            await _courseRepository.Received(1).GetFilteredCoursesAsync("Course 1", null, 1, 10);
        }
    }
}
