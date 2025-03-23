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
    public class GetLessonByIdQueryHandlerTests
    {
        private readonly ILessonRepository _lessonRepository;
        private readonly ICourseRepository _courseRepository;
        private readonly IProfessorRepository _professorRepository;
        private readonly IStudentCourseRepository _studentCourseRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly IMapper _mapper;
        private readonly GetLessonByIdQueryHandler _handler;

        public GetLessonByIdQueryHandlerTests()
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

            _handler = new GetLessonByIdQueryHandler(_lessonRepository, _mapper, _courseRepository, _professorRepository, _studentCourseRepository, _studentRepository);
        }

   

        [Fact]
        public async Task Handle_Should_Return_LessonDTO_Without_Professor_When_Professor_Does_Not_Exist()
        {
            // Arrange
            var lessonId = Guid.NewGuid();
            var courseId = Guid.NewGuid();
            var professorId = Guid.NewGuid();
            var lesson = new Lesson { Id = lessonId, Name = "Lesson 1", Content = "Content 1", CourseId = courseId };
            var course = new Course { Id = courseId, Name = "Course 1", ProfessorId = professorId };

            _lessonRepository.GetLessonByIdAsync(lessonId).Returns(Task.FromResult(lesson));
            _courseRepository.GetCourseByIdAsync(courseId).Returns(Task.FromResult(course));
            _professorRepository.GetProfessorByIdAsync(professorId).Returns(Task.FromResult<Professor>(null));

            var query = new GetLessonByIdQuery { Id = lessonId };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(lessonId);
            result.Course.Should().NotBeNull();
            result.Course.Professor.Should().BeNull();
        }
/*
        [Fact]
        public async Task Handle_Should_Return_LessonDTO_Without_StudentCourses_When_StudentCourses_Do_Not_Exist()
        {
            // Arrange
            var lessonId = Guid.NewGuid();
            var courseId = Guid.NewGuid();
            var lesson = new Lesson { Id = lessonId, Name = "Lesson 1", Content = "Content 1", CourseId = courseId };
            var course = new Course { Id = courseId, Name = "Course 1" };

            _lessonRepository.GetLessonByIdAsync(lessonId).Returns(Task.FromResult(lesson));
            _courseRepository.GetCourseByIdAsync(courseId).Returns(Task.FromResult(course));
            _studentCourseRepository.GetAllStudentCoursesAsync().Returns(Task.FromResult<IEnumerable<StudentCourse>>(null));

            var query = new GetLessonByIdQuery { Id = lessonId };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(lessonId);
            result.Course.Should().NotBeNull();
            result.Course.StudentCourses.Should().BeNull();
        }*/
    }
}
