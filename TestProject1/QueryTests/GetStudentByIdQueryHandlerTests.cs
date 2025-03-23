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
    public class GetStudentByIdQueryHandlerTests
    {
        private readonly IStudentRepository _studentRepository;
        private readonly ICourseRepository _courseRepository;
        private readonly IProfessorRepository _professorRepository;
        private readonly ILessonRepository _lessonRepository;
        private readonly IMapper _mapper;
        private readonly GetStudentByIdQueryHandler _handler;

        public GetStudentByIdQueryHandlerTests()
        {
            _studentRepository = Substitute.For<IStudentRepository>();
            _courseRepository = Substitute.For<ICourseRepository>();
            _professorRepository = Substitute.For<IProfessorRepository>();
            _lessonRepository = Substitute.For<ILessonRepository>();

            // Setup AutoMapper
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Student, StudentDTO>();
                cfg.CreateMap<Course, CourseDTO>();
                cfg.CreateMap<Professor, ProfessorDTO>();
                cfg.CreateMap<Lesson, LessonDTO>();
                cfg.CreateMap<StudentCourse, StudentCourseDTO>();
            });
            _mapper = config.CreateMapper();

            _handler = new GetStudentByIdQueryHandler(_studentRepository, _lessonRepository, _courseRepository, _professorRepository, _mapper);
        }

       

        [Fact]
        public async Task Handle_Should_Return_Empty_StudentDTO_When_Student_Not_Found()
        {
            // Arrange
            var studentId = Guid.NewGuid();
            _studentRepository.GetStudentByIdAsync(studentId).Returns(Task.FromResult<Student>(null));

            var query = new GetStudentByIdQuery { Id = studentId };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(Guid.Empty);
        }
      
    }
}
