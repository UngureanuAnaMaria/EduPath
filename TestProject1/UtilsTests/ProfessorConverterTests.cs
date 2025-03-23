using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using Application.DTOs;
using Application.Utils;
using Domain.Entities;
using Xunit;

public class ProfessorConverterTests
{
    private readonly JsonSerializerOptions _options;

    public ProfessorConverterTests()
    {
        _options = new JsonSerializerOptions
        {
            Converters = { new ProfessorConverter() },
            WriteIndented = true
        };
    }

    [Fact]
    public void Write_ShouldSerializeProfessorDTOCorrectly()
    {
        // Arrange
        var professorDto = new ProfessorDTO
        {
            Id = Guid.NewGuid(),
            Name = "John Doe",
            Email = "john.doe@example.com",
            Password = "password123",
            Status = true,
            CreatedAt = DateTime.UtcNow,
            LastLogin = DateTime.UtcNow,
            Courses = new List<CourseDTO>
            {
                new CourseDTO
                {
                    Id = Guid.NewGuid(),
                    Name = "Course 1",
                    Description = "Description 1",
                    StudentCourses = new List<StudentCourseDTO>
                    {
                        new StudentCourseDTO
                        {
                            Id = Guid.NewGuid(),
                            StudentId = Guid.NewGuid(),
                            Student = new StudentDTO
                            {
                                Id = Guid.NewGuid(),
                                Name = "Student 1",
                                Email = "student1@example.com",
                                Password = "password123",
                                Status = true,
                                CreatedAt = DateTime.UtcNow,
                                LastLogin = DateTime.UtcNow
                            }
                        }
                    },
                    Lessons = new List<LessonDTO>
                    {
                        new LessonDTO
                        {
                            Id = Guid.NewGuid(),
                            Name = "Lesson 1",
                            Content = "Lesson content",
                            CourseId = Guid.NewGuid()
                        }
                    }
                }
            }
        };

        // Act
        var json = JsonSerializer.Serialize(professorDto, _options);

        // Assert
        Assert.Contains("\"id\": \"" + professorDto.Id.ToString() + "\"", json);
        Assert.Contains("\"name\": \"" + professorDto.Name + "\"", json);
        Assert.Contains("\"email\": \"" + professorDto.Email + "\"", json);
        Assert.Contains("\"courses\"", json);
        Assert.Contains("\"studentCourses\"", json);
        Assert.Contains("\"lessons\"", json);
    }

    [Fact]
    public void Write_ShouldHandleProfessorWithNoCourses()
    {
        // Arrange
        var professorDto = new ProfessorDTO
        {
            Id = Guid.NewGuid(),
            Name = "John Doe",
            Email = "john.doe@example.com",
            Password = "password123",
            Status = true,
            CreatedAt = DateTime.UtcNow,
            LastLogin = DateTime.UtcNow,
            Courses = null // No courses
        };

        // Act
        var json = JsonSerializer.Serialize(professorDto, _options);

        // Assert
        Assert.Contains("\"id\": \"" + professorDto.Id.ToString() + "\"", json);
        Assert.Contains("\"courses\": []", json); // No courses field
    }

    [Fact]
    public void Write_ShouldHandleProfessorWithEmptyCourses()
    {
        // Arrange
        var professorDto = new ProfessorDTO
        {
            Id = Guid.NewGuid(),
            Name = "John Doe",
            Email = "john.doe@example.com",
            Password = "password123",
            Status = true,
            CreatedAt = DateTime.UtcNow,
            LastLogin = DateTime.UtcNow,
            Courses = new List<CourseDTO>() // Empty courses list
        };

        // Act
        var json = JsonSerializer.Serialize(professorDto, _options);

        // Assert
        Assert.Contains("\"id\": \"" + professorDto.Id.ToString() + "\"", json);
        Assert.Contains("\"courses\": []", json); // Empty courses array
    }

    [Fact]
    public void Write_ShouldHandleProfessorWithNullStudentCourses()
    {
        // Arrange
        var professorDto = new ProfessorDTO
        {
            Id = Guid.NewGuid(),
            Name = "John Doe",
            Email = "john.doe@example.com",
            Password = "password123",
            Status = true,
            CreatedAt = DateTime.UtcNow,
            LastLogin = DateTime.UtcNow,
            Courses = new List<CourseDTO>
            {
                new CourseDTO
                {
                    Id = Guid.NewGuid(),
                    Name = "Course 1",
                    Description = "Description 1",
                    StudentCourses = null, // No student courses
                    Lessons = new List<LessonDTO>()
                }
            }
        };

        // Act
        var json = JsonSerializer.Serialize(professorDto, _options);

        // Assert
        Assert.Contains("\"courses\"", json);
        Assert.Contains("\"studentCourses\": []", json); // Empty studentCourses array
    }

    [Fact]
    public void Write_ShouldHandleProfessorWithNullLessons()
    {
        // Arrange
        var professorDto = new ProfessorDTO
        {
            Id = Guid.NewGuid(),
            Name = "John Doe",
            Email = "john.doe@example.com",
            Password = "password123",
            Status = true,
            CreatedAt = DateTime.UtcNow,
            LastLogin = DateTime.UtcNow,
            Courses = new List<CourseDTO>
            {
                new CourseDTO
                {
                    Id = Guid.NewGuid(),
                    Name = "Course 1",
                    Description = "Description 1",
                    StudentCourses = new List<StudentCourseDTO>(),
                    Lessons = null // No lessons
                }
            }
        };

        // Act
        var json = JsonSerializer.Serialize(professorDto, _options);

        // Assert
        Assert.Contains("\"courses\"", json);
        Assert.Contains("\"lessons\": []", json); // Empty lessons array
    }

    
}
