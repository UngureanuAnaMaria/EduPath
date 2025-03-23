using System;
using AutoMapper;
using Application.DTOs;
using Application.Use_Cases.Commands.Create;
using Application.Use_Cases.Commands.Update;
using Domain.Entities;
using Xunit;
using Application.Utils;

public class MappingProfileTests
{
    private readonly IMapper _mapper;

    public MappingProfileTests()
    {
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
        _mapper = configuration.CreateMapper();
    }

    #region Student Mappings

    [Fact]
    public void StudentToStudentDTO_MapsCorrectly()
    {
        // Arrange
        var student = new Student
        {
            Id = Guid.NewGuid(),
            Name = "John Doe",
            Email = "john.doe@example.com",
            Password = "password",
            Status = true,
            CreatedAt = DateTime.UtcNow,
            LastLogin = DateTime.UtcNow
        };

        // Act
        var studentDTO = _mapper.Map<StudentDTO>(student);

        // Assert
        Assert.Equal(student.Id, studentDTO.Id);
        Assert.Equal(student.Name, studentDTO.Name);
        Assert.Equal(student.Email, studentDTO.Email);
        Assert.Equal(student.Status, studentDTO.Status);
        Assert.Equal(student.CreatedAt, studentDTO.CreatedAt);
        Assert.Equal(student.LastLogin, studentDTO.LastLogin);
    }

    [Fact]
    public void StudentDTOToStudent_MapsCorrectly()
    {
        // Arrange
        var studentDTO = new StudentDTO
        {
            Id = Guid.NewGuid(),
            Name = "John Doe",
            Email = "john.doe@example.com",
            Status = true,
            CreatedAt = DateTime.UtcNow,
            LastLogin = DateTime.UtcNow
        };

        // Act
        var student = _mapper.Map<Student>(studentDTO);

        // Assert
        Assert.Equal(studentDTO.Id, student.Id);
        Assert.Equal(studentDTO.Name, student.Name);
        Assert.Equal(studentDTO.Email, student.Email);
        Assert.Equal(studentDTO.Status, student.Status);
        Assert.Equal(studentDTO.CreatedAt, student.CreatedAt);
        Assert.Equal(studentDTO.LastLogin, student.LastLogin);
    }

    #endregion

    #region Professor Mappings

    [Fact]
    public void ProfessorToProfessorDTO_MapsCorrectly()
    {
        // Arrange
        var professor = new Professor
        {
            Id = Guid.NewGuid(),
            Name = "Dr. Alice Smith",
            Email = "alice.smith@example.com",
            Password = "password",
            Status = true,
            CreatedAt = DateTime.UtcNow,
            LastLogin = DateTime.UtcNow
        };

        // Act
        var professorDTO = _mapper.Map<ProfessorDTO>(professor);

        // Assert
        Assert.Equal(professor.Id, professorDTO.Id);
        Assert.Equal(professor.Name, professorDTO.Name);
        Assert.Equal(professor.Email, professorDTO.Email);
        Assert.Equal(professor.Status, professorDTO.Status);
        Assert.Equal(professor.CreatedAt, professorDTO.CreatedAt);
        Assert.Equal(professor.LastLogin, professorDTO.LastLogin);
    }

    [Fact]
    public void ProfessorDTOToProfessor_MapsCorrectly()
    {
        // Arrange
        var professorDTO = new ProfessorDTO
        {
            Id = Guid.NewGuid(),
            Name = "Dr. Alice Smith",
            Email = "alice.smith@example.com",
            Status = true,
            CreatedAt = DateTime.UtcNow,
            LastLogin = DateTime.UtcNow
        };

        // Act
        var professor = _mapper.Map<Professor>(professorDTO);

        // Assert
        Assert.Equal(professorDTO.Id, professor.Id);
        Assert.Equal(professorDTO.Name, professor.Name);
        Assert.Equal(professorDTO.Email, professor.Email);
        Assert.Equal(professorDTO.Status, professor.Status);
        Assert.Equal(professorDTO.CreatedAt, professor.CreatedAt);
        Assert.Equal(professorDTO.LastLogin, professor.LastLogin);
    }

    #endregion

    #region Course Mappings

    [Fact]
    public void CourseToCourseDTO_MapsCorrectly()
    {
        // Arrange
        var course = new Course
        {
            Id = Guid.NewGuid(),
            Name = "Data Structures",
            Description = "A course about data structures",
            ProfessorId = Guid.NewGuid()
        };

        // Act
        var courseDTO = _mapper.Map<CourseDTO>(course);

        // Assert
        Assert.Equal(course.Id, courseDTO.Id);
        Assert.Equal(course.Name, courseDTO.Name);
        Assert.Equal(course.Description, courseDTO.Description);
        Assert.Equal(course.ProfessorId, courseDTO.ProfessorId);
    }

    [Fact]
    public void CourseDTOToCourse_MapsCorrectly()
    {
        // Arrange
        var courseDTO = new CourseDTO
        {
            Id = Guid.NewGuid(),
            Name = "Data Structures",
            Description = "A course about data structures",
            ProfessorId = Guid.NewGuid()
        };

        // Act
        var course = _mapper.Map<Course>(courseDTO);

        // Assert
        Assert.Equal(courseDTO.Id, course.Id);
        Assert.Equal(courseDTO.Name, course.Name);
        Assert.Equal(courseDTO.Description, course.Description);
        Assert.Equal(courseDTO.ProfessorId, course.ProfessorId);
    }

    #endregion

    #region Lesson Mappings

    [Fact]
    public void LessonToLessonDTO_MapsCorrectly()
    {
        // Arrange
        var lesson = new Lesson
        {
            Id = Guid.NewGuid(),
            Name = "Algorithms",
            Content = "An introduction to algorithms",
            CourseId = Guid.NewGuid()
        };

        // Act
        var lessonDTO = _mapper.Map<LessonDTO>(lesson);

        // Assert
        Assert.Equal(lesson.Id, lessonDTO.Id);
        Assert.Equal(lesson.Name, lessonDTO.Name);
        Assert.Equal(lesson.Content, lessonDTO.Content);
        Assert.Equal(lesson.CourseId, lessonDTO.CourseId);
    }

    [Fact]
    public void LessonDTOToLesson_MapsCorrectly()
    {
        // Arrange
        var lessonDTO = new LessonDTO
        {
            Id = Guid.NewGuid(),
            Name = "Algorithms",
            Content = "An introduction to algorithms",
            CourseId = Guid.NewGuid()
        };

        // Act
        var lesson = _mapper.Map<Lesson>(lessonDTO);

        // Assert
        Assert.Equal(lessonDTO.Id, lesson.Id);
        Assert.Equal(lessonDTO.Name, lesson.Name);
        Assert.Equal(lessonDTO.Content, lesson.Content);
        Assert.Equal(lessonDTO.CourseId, lesson.CourseId);
    }

    

    #endregion
}
