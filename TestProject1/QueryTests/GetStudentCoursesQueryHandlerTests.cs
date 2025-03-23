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
    public class GetStudentCoursesQueryHandlerTests
    {
        private readonly IStudentCourseRepository _repository;
        private readonly IMapper _mapper;
        private readonly GetStudentCoursesQueryHandler _handler;

        public GetStudentCoursesQueryHandlerTests()
        {
            _repository = Substitute.For<IStudentCourseRepository>();

            // Setup AutoMapper
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<StudentCourse, StudentCourseDTO>();
                cfg.CreateMap<Student, StudentDTO>();
                cfg.CreateMap<Course, CourseDTO>();
            });
            _mapper = config.CreateMapper();

            _handler = new GetStudentCoursesQueryHandler(_repository, _mapper);
        }

        [Fact]
        public async Task Handle_Should_Return_Empty_Response_When_No_StudentCourses_Found()
        {
            // Arrange
            _repository.GetFilteredStudentCoursesAsync(null, null, 1, 10).Returns((Enumerable.Empty<StudentCourse>(), 0));
            var query = new GetStudentCoursesQuery { PageNumber = 1, PageSize = 10 };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }

        [Fact]
        public async Task Handle_Should_Return_StudentCourses()
        {
            // Arrange
            var studentCourseId = Guid.NewGuid();
            var studentId = Guid.NewGuid();
            var courseId = Guid.NewGuid();

            var studentCourses = new List<StudentCourse>
            {
                new StudentCourse
                {
                    Id = studentCourseId,
                    StudentId = studentId,
                    CourseId = courseId,
                    Student = new Student
                    {
                        Id = studentId,
                        Name = "Student 1",
                        Email = "student1@example.com",
                        Password = "password",
                        Status = true,
                        CreatedAt = DateTime.UtcNow,
                        LastLogin = DateTime.UtcNow
                    },
                    Course = new Course
                    {
                        Id = courseId,
                        Name = "Course 1",
                        Description = "Description 1"
                    }
                }
            };

            _repository.GetFilteredStudentCoursesAsync(null, null, 1, 10).Returns((studentCourses, 1));
            var query = new GetStudentCoursesQuery { PageNumber = 1, PageSize = 10 };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(1);

            var studentCourseDTO = result.First();
            studentCourseDTO.Id.Should().Be(studentCourseId);
            studentCourseDTO.StudentId.Should().Be(studentId);
            studentCourseDTO.CourseId.Should().Be(courseId);
            studentCourseDTO.Student.Should().NotBeNull();
            studentCourseDTO.Student.Id.Should().Be(studentId);
            studentCourseDTO.Course.Should().NotBeNull();
            studentCourseDTO.Course.Id.Should().Be(courseId);
        }
    }
}
