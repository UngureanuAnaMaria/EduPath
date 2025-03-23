using Application.DTOs;
using Application.Use_Cases.QueryHandlers;
using Application.Use_Cases.Queries;
using AutoMapper;
using Domain.Entities;
using Domain.Repositories;
using FluentAssertions;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace TestProject1.QueryTests
{
    public class GetProfessorsQueryHandlerTests
    {
        private readonly IProfessorRepository _professorRepository;
        private readonly ICourseRepository _courseRepository;
        private readonly IStudentCourseRepository _studentCourseRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly ILessonRepository _lessonRepository;
        private readonly IMapper _mapper;
        private readonly GetProfessorsQueryHandler _handler;

        public GetProfessorsQueryHandlerTests()
        {
            _professorRepository = Substitute.For<IProfessorRepository>();
            _courseRepository = Substitute.For<ICourseRepository>();
            _studentCourseRepository = Substitute.For<IStudentCourseRepository>();
            _studentRepository = Substitute.For<IStudentRepository>();
            _lessonRepository = Substitute.For<ILessonRepository>();

            // Setup AutoMapper
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Professor, ProfessorDTO>();
                cfg.CreateMap<Course, CourseDTO>();
                cfg.CreateMap<StudentCourse, StudentCourseDTO>();
                cfg.CreateMap<Student, StudentDTO>();
                cfg.CreateMap<Lesson, LessonDTO>();
            });
            _mapper = config.CreateMapper();

            _handler = new GetProfessorsQueryHandler(_professorRepository, _lessonRepository, _mapper, _courseRepository, _studentCourseRepository, _studentRepository);
        }

        [Fact]
        public async Task Handle_Should_Return_Empty_List_When_No_Professors_Found()
        {
            // Arrange
            _professorRepository.GetFilteredProfessorsAsync(Arg.Any<string>(), Arg.Any<bool?>(), Arg.Any<int>(), Arg.Any<int>())
                .Returns(Task.FromResult((Enumerable.Empty<Professor>(), 0)));

            var query = new GetProfessorsQuery { Name = "NonExistent", Status = true, PageNumber = 1, PageSize = 10 };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().BeEmpty();
        }

        [Fact]
        public async Task Handle_Should_Return_Professors_With_Courses_And_Students()
        {
            // Arrange
            var professorId = Guid.NewGuid();
            var professor = new Professor
            {
                Id = professorId,
                Name = "Professor 1",
                Email = "professor1@example.com",
                Password = "password",
                Status = true,
                CreatedAt = DateTime.UtcNow,
                LastLogin = DateTime.UtcNow
            };

            var courseId = Guid.NewGuid();
            var course = new Course
            {
                Id = courseId,
                Name = "Course 1",
                Description = "Description 1",
                ProfessorId = professorId
            };

            var studentId = Guid.NewGuid();
            var student = new Student
            {
                Id = studentId,
                Name = "Student 1",
                Email = "student1@example.com",
                Password = "password",
                Status = true,
                CreatedAt = DateTime.UtcNow,
                LastLogin = DateTime.UtcNow
            };

            var studentCourse = new StudentCourse
            {
                Id = Guid.NewGuid(),
                StudentId = studentId,
                CourseId = courseId
            };

            var lesson = new Lesson
            {
                Id = Guid.NewGuid(),
                Name = "Lesson 1",
                Content = "Content 1",
                CourseId = courseId
            };

            _professorRepository.GetFilteredProfessorsAsync(Arg.Any<string>(), Arg.Any<bool?>(), Arg.Any<int>(), Arg.Any<int>())
                .Returns(Task.FromResult(((IEnumerable<Professor>)new List<Professor> { professor }, 1)));
            _courseRepository.GetAllCoursesAsync().Returns(Task.FromResult<IEnumerable<Course>>(new List<Course> { course }));
            _studentCourseRepository.GetAllStudentCoursesAsync().Returns(Task.FromResult<IEnumerable<StudentCourse>>(new List<StudentCourse> { studentCourse }));
            _studentRepository.GetStudentByIdAsync(studentId).Returns(Task.FromResult(student));
            _lessonRepository.GetAllLessonsAsync().Returns(Task.FromResult<IEnumerable<Lesson>>(new List<Lesson> { lesson }));

            var query = new GetProfessorsQuery { Name = "Professor 1", Status = true, PageNumber = 1, PageSize = 10 };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Count.Should().Be(1);
            result[0].Id.Should().Be(professorId);
            result[0].Courses.Should().NotBeNull();
            result[0].Courses.Count.Should().Be(1);
            result[0].Courses[0].StudentCourses.Should().NotBeNull();
            result[0].Courses[0].StudentCourses.Count.Should().Be(1);
            result[0].Courses[0].Lessons.Should().NotBeNull();
            result[0].Courses[0].Lessons.Count.Should().Be(1);
        }
    }
}
