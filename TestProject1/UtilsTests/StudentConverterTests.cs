using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using Application.DTOs;
using Application.Utils;
using Domain.Entities;
using Xunit;

public class StudentConverterTests
{
    private readonly JsonSerializerOptions _options;

    public StudentConverterTests()
    {
        _options = new JsonSerializerOptions
        {
            Converters = { new StudentConverter() },
            WriteIndented = true
        };
    }

    [Fact]
    public void Write_ShouldSerializeStudentDTOCorrectly()
    {
        // Arrange
        var studentDto = new StudentDTO
        {
            Id = Guid.NewGuid(),
            Name = "Jane Doe",
            Email = "jane.doe@example.com",
            Password = "password123",
            Status = true,
            CreatedAt = DateTime.UtcNow,
            LastLogin = DateTime.UtcNow,
            StudentCourses = new List<StudentCourseDTO>
            {
                new StudentCourseDTO
                {
                    Id = Guid.NewGuid(),
                    CourseId = Guid.NewGuid(),
                    Course = new CourseDTO
                    {
                        Id = Guid.NewGuid(),
                        Name = "Course 1",
                        Description = "Description 1",
                        ProfessorId = Guid.NewGuid(),
                        Professor = new ProfessorDTO
                        {
                            Id = Guid.NewGuid(),
                            Name = "Professor 1",
                            Email = "professor1@example.com",
                            Password = "password123",
                            Status = true,
                            CreatedAt = DateTime.UtcNow,
                            LastLogin = DateTime.UtcNow
                        },
                        Lessons = new List<LessonDTO>
                        {
                            new LessonDTO
                            {
                                Id = Guid.NewGuid(),
                                Name = "Lesson 1",
                                Content = "Lesson Content",
                                CourseId = Guid.NewGuid()
                            }
                        }
                    }
                }
            }
        };

        // Act
        var json = JsonSerializer.Serialize(studentDto, _options);

        // Assert
        Assert.Contains("\"id\": \"" + studentDto.Id.ToString() + "\"", json);
        Assert.Contains("\"name\": \"" + studentDto.Name + "\"", json);
        Assert.Contains("\"email\": \"" + studentDto.Email + "\"", json);
        Assert.Contains("\"studentCourses\"", json);
        Assert.Contains("\"courseId\": \"" + studentDto.StudentCourses[0].CourseId.ToString() + "\"", json);
        Assert.Contains("\"professorId\": \"" + studentDto.StudentCourses[0].Course.ProfessorId.ToString() + "\"", json);
        Assert.Contains("\"lessons\"", json);
    }



    [Fact]
    public void Write_ShouldHandleStudentWithEmptyCourses()
    {
        // Arrange
        var studentDto = new StudentDTO
        {
            Id = Guid.NewGuid(),
            Name = "Jane Doe",
            Email = "jane.doe@example.com",
            Password = "password123",
            Status = true,
            CreatedAt = DateTime.UtcNow,
            LastLogin = DateTime.UtcNow,
            StudentCourses = new List<StudentCourseDTO>() // Empty courses list
        };

        // Act
        var json = JsonSerializer.Serialize(studentDto, _options);

        // Assert
        Assert.Contains("\"id\": \"" + studentDto.Id.ToString() + "\"", json);
        Assert.Contains("\"studentCourses\": []", json); // Empty studentCourses array
    }

    [Fact]
    public void Write_ShouldHandleStudentWithNullProfessorInCourse()
    {
        // Arrange
        var studentDto = new StudentDTO
        {
            Id = Guid.NewGuid(),
            Name = "Jane Doe",
            Email = "jane.doe@example.com",
            Password = "password123",
            Status = true,
            CreatedAt = DateTime.UtcNow,
            LastLogin = DateTime.UtcNow,
            StudentCourses = new List<StudentCourseDTO>
            {
                new StudentCourseDTO
                {
                    Id = Guid.NewGuid(),
                    CourseId = Guid.NewGuid(),
                    Course = new CourseDTO
                    {
                        Id = Guid.NewGuid(),
                        Name = "Course 1",
                        Description = "Description 1",
                        ProfessorId = null, // No professor
                        Professor = null, // No professor object
                        Lessons = new List<LessonDTO>
                        {
                            new LessonDTO
                            {
                                Id = Guid.NewGuid(),
                                Name = "Lesson 1",
                                Content = "Lesson Content",
                                CourseId = Guid.NewGuid()
                            }
                        }
                    }
                }
            }
        };

        // Act
        var json = JsonSerializer.Serialize(studentDto, _options);

        // Assert
        Assert.Contains("\"studentCourses\"", json);
        Assert.DoesNotContain("\"professorId\"", json); // No professorId field
        Assert.Contains("\"lessons\"", json); // Lessons should still be serialized
    }

    

    [Fact]
    public void Write_ShouldHandleStudentWithNullValuesForOptionalProperties()
    {
        // Arrange
        var studentDto = new StudentDTO
        {
            Id = Guid.NewGuid(),
            Name = "Jane Doe",
            Email = "jane.doe@example.com",
            Password = "password123",
            Status = true,
            CreatedAt = DateTime.UtcNow,
            LastLogin = null, // Null LastLogin
            StudentCourses = null // No student courses
        };

        // Act
        var json = JsonSerializer.Serialize(studentDto, _options);

        // Assert
        Assert.Contains("\"lastLogin\": null", json); // Check for null lastLogin
    }

    
}
