using Application.DTOs;
using Application.Responses;
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
    public class GetStudentsQueryHandlerTests
    {
        private readonly IStudentRepository _studentRepository;
        private readonly ICourseRepository _courseRepository;
        private readonly IProfessorRepository _professorRepository;
        private readonly ILessonRepository _lessonRepository;
        private readonly IMapper _mapper;
        private readonly GetStudentsQueryHandler _handler;

        public GetStudentsQueryHandlerTests()
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

            _handler = new GetStudentsQueryHandler(_studentRepository, _lessonRepository, _courseRepository, _professorRepository, _mapper);
        }

        [Fact]
        public async Task Handle_Should_Return_Empty_Response_When_No_Students_Found()
        {
            // Arrange
            _studentRepository.GetFilteredStudentsAsync(null, null, 1, 10).Returns((Enumerable.Empty<Student>(), 0));
            var query = new GetStudentsQuery { PageNumber = 1, PageSize = 10 };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Students.Should().BeEmpty();
            result.TotalCount.Should().Be(0);
        }

      
    }
}
