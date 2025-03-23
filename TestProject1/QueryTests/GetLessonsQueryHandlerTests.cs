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
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace TestProject1.QueryTests
{
    public class GetLessonsQueryHandlerTests
    {
        private readonly ILessonRepository _lessonRepository;
        private readonly ICourseRepository _courseRepository;
        private readonly IProfessorRepository _professorRepository;
        private readonly IStudentCourseRepository _studentCourseRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly IMapper _mapper;
        private readonly GetLessonsQueryHandler _handler;

        public GetLessonsQueryHandlerTests()
        {
            _lessonRepository = Substitute.For<ILessonRepository>();
            _courseRepository = Substitute.For<ICourseRepository>();
            _professorRepository = Substitute.For<IProfessorRepository>();
            _studentCourseRepository = Substitute.For<IStudentCourseRepository>();
            _studentRepository = Substitute.For<IStudentRepository>();

            // Setup AutoMapper
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Lesson, LessonDTO>();
                cfg.CreateMap<Course, CourseDTO>();
                cfg.CreateMap<Professor, ProfessorDTO>();
                cfg.CreateMap<Student, StudentDTO>();
                cfg.CreateMap<StudentCourse, StudentCourseDTO>();
            });
            _mapper = config.CreateMapper();

            _handler = new GetLessonsQueryHandler(_lessonRepository, _mapper, _courseRepository, _professorRepository, _studentCourseRepository, _studentRepository);
        }

        [Fact]
        public async Task Handle_Should_Return_Lessons_When_Lessons_Exist()
        {
            // Arrange
            var lessonId = Guid.NewGuid();
            var courseId = Guid.NewGuid();
            var professorId = Guid.NewGuid();
            var studentId = Guid.NewGuid();
            var lessons = new List<Lesson>
            {
                new Lesson { Id = lessonId, Name = "Lesson 1", Content = "Content 1", CourseId = courseId }
            };
            var course = new Course { Id = courseId, Name = "Course 1", ProfessorId = professorId };
            var professor = new Professor { Id = professorId, Name = "Professor 1" };
            var studentCourse = new StudentCourse { Id = Guid.NewGuid(), StudentId = studentId, CourseId = courseId };
            var student = new Student { Id = studentId, Name = "Student 1" };

            _lessonRepository.GetFilteredLessonsAsync(null, null, 1, 10).Returns((lessons, 1));
            _courseRepository.GetCourseByIdAsync(courseId).Returns(Task.FromResult(course));
            _professorRepository.GetProfessorByIdAsync(professorId).Returns(Task.FromResult(professor));
            _studentCourseRepository.GetAllStudentCoursesAsync().Returns(Task.FromResult<IEnumerable<StudentCourse>>(new List<StudentCourse> { studentCourse }));
            _studentRepository.GetStudentByIdAsync(studentId).Returns(Task.FromResult(student));

            var query = new GetLessonsQuery { PageNumber = 1, PageSize = 10 };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(1);
            result[0].Id.Should().Be(lessonId);
            result[0].Course.Should().NotBeNull();
            result[0].Course.Professor.Should().NotBeNull();
            result[0].Course.StudentCourses.Should().NotBeNull();
            result[0].Course.StudentCourses.Should().HaveCount(1);
            result[0].Course.StudentCourses[0].Student.Should().NotBeNull();
        }

        [Fact]
        public async Task Handle_Should_Return_Empty_List_When_No_Lessons_Exist()
        {
            // Arrange
            _lessonRepository.GetFilteredLessonsAsync(null, null, 1, 10).Returns((new List<Lesson>(), 0));

            var query = new GetLessonsQuery { PageNumber = 1, PageSize = 10 };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }

        [Fact]
        public async Task Handle_Should_Return_Lessons_Without_Course_When_Course_Does_Not_Exist()
        {
            // Arrange
            var lessonId = Guid.NewGuid();
            var courseId = Guid.NewGuid();
            var lessons = new List<Lesson>
            {
                new Lesson { Id = lessonId, Name = "Lesson 1", Content = "Content 1", CourseId = courseId }
            };

            _lessonRepository.GetFilteredLessonsAsync(null, null, 1, 10).Returns((lessons, 1));
            _courseRepository.GetCourseByIdAsync(courseId).Returns(Task.FromResult<Course>(null));

            var query = new GetLessonsQuery { PageNumber = 1, PageSize = 10 };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(1);
            result[0].Id.Should().Be(lessonId);
            result[0].Course.Should().BeNull();
        }

        [Fact]
        public async Task Handle_Should_Return_Lessons_Without_Professor_When_Professor_Does_Not_Exist()
        {
            // Arrange
            var lessonId = Guid.NewGuid();
            var courseId = Guid.NewGuid();
            var professorId = Guid.NewGuid();
            var lessons = new List<Lesson>
            {
                new Lesson { Id = lessonId, Name = "Lesson 1", Content = "Content 1", CourseId = courseId }
            };
            var course = new Course { Id = courseId, Name = "Course 1", ProfessorId = professorId };

            _lessonRepository.GetFilteredLessonsAsync(null, null, 1, 10).Returns((lessons, 1));
            _courseRepository.GetCourseByIdAsync(courseId).Returns(Task.FromResult(course));
            _professorRepository.GetProfessorByIdAsync(professorId).Returns(Task.FromResult<Professor>(null));

            var query = new GetLessonsQuery { PageNumber = 1, PageSize = 10 };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(1);
            result[0].Id.Should().Be(lessonId);
            result[0].Course.Should().NotBeNull();
            result[0].Course.Professor.Should().BeNull();
        }
/*
        [Fact]
        public async Task Handle_Should_Return_Lessons_Without_StudentCourses_When_StudentCourses_Do_Not_Exist()
        {
            // Arrange
            var lessonId = Guid.NewGuid();
            var courseId = Guid.NewGuid();
            var lessons = new List<Lesson>
            {
                new Lesson { Id = lessonId, Name = "Lesson 1", Content = "Content 1", CourseId = courseId }
            };
            var course = new Course { Id = courseId, Name = "Course 1" };

            _lessonRepository.GetFilteredLessonsAsync(null, null, 1, 10).Returns((lessons, 1));
            _courseRepository.GetCourseByIdAsync(courseId).Returns(Task.FromResult(course));
            _studentCourseRepository.GetAllStudentCoursesAsync().Returns(Task.FromResult<IEnumerable<StudentCourse>>(null));

            var query = new GetLessonsQuery { PageNumber = 1, PageSize = 10 };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(1);
            result[0].Id.Should().Be(lessonId);
            result[0].Course.Should().NotBeNull();
            result[0].Course.StudentCourses.Should().BeNull();
        }*/
    }
}
