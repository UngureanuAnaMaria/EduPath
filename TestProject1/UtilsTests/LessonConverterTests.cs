using System;
using System.Collections.Generic;
using System.Text.Json;
using Application.DTOs;
using Application.Utils;
using Xunit;

public class LessonConverterTests
{
    [Fact]
    public void Write_LessonDTO_WritesExpectedJson()
    {
        // Arrange
        var lessonDto = new LessonDTO
        {
            Id = Guid.NewGuid(),
            Name = "Introduction to Quantum Mechanics",
            Content = "Basic concepts in quantum physics",
            CourseId = Guid.NewGuid(),
            Course = new CourseDTO
            {
                Id = Guid.NewGuid(),
                Name = "Physics 101",
                Description = "An introductory course to Physics",
                ProfessorId = Guid.NewGuid(),
                Professor = new ProfessorDTO
                {
                    Id = Guid.NewGuid(),
                    Name = "Dr. Albert Einstein",
                    Email = "einstein@example.com",
                    Password = "password",
                    Status = true,
                    CreatedAt = DateTime.UtcNow,
                    LastLogin = DateTime.UtcNow
                },
                StudentCourses = new List<StudentCourseDTO>
                {
                    new StudentCourseDTO
                    {
                        Id = Guid.NewGuid(),
                        StudentId = Guid.NewGuid(),
                        Student = new StudentDTO
                        {
                            Id = Guid.NewGuid(),
                            Name = "John Doe",
                            Email = "johndoe@example.com",
                            Password = "password",
                            Status = true,
                            CreatedAt = DateTime.UtcNow,
                            LastLogin = DateTime.UtcNow
                        }
                    }
                }
            }
        };

        var jsonConverter = new LessonConverter();
        var options = new JsonSerializerOptions
        {
            Converters = { jsonConverter }
        };

        // Act
        string json;
        using (var stream = new System.IO.MemoryStream())
        {
            using (var writer = new Utf8JsonWriter(stream, new JsonWriterOptions { Indented = true }))
            {
                jsonConverter.Write(writer, lessonDto, options);
            }
            json = System.Text.Encoding.UTF8.GetString(stream.ToArray());
        }

        // Assert
        Assert.Contains("\"id\"", json);
        Assert.Contains("\"name\"", json);
        Assert.Contains("\"content\"", json);
        Assert.Contains("\"courseId\"", json);
        Assert.Contains("\"course\"", json);
        Assert.Contains("\"professor\"", json);
        Assert.Contains("\"studentCourses\"", json);
    }

    [Fact]
    public void Write_LessonDTO_WithNullValues_ExcludesNullProperties()
    {
        // Arrange
        var lessonDto = new LessonDTO
        {
            Id = Guid.NewGuid(),
            Name = "Thermodynamics Basics",
            Content = "Introduction to heat and work",
            CourseId = Guid.NewGuid(),
            Course = new CourseDTO
            {
                Id = Guid.NewGuid(),
                Name = "Thermodynamics",
                Description = "An in-depth study of thermodynamics",
                ProfessorId = null,  // Null professor
                Professor = null,  // Null professor
                StudentCourses = null  // No student courses
            }
        };

        var jsonConverter = new LessonConverter();
        var options = new JsonSerializerOptions
        {
            Converters = { jsonConverter }
        };

        // Act
        string json;
        using (var stream = new System.IO.MemoryStream())
        {
            using (var writer = new Utf8JsonWriter(stream, new JsonWriterOptions { Indented = true }))
            {
                jsonConverter.Write(writer, lessonDto, options);
            }
            json = System.Text.Encoding.UTF8.GetString(stream.ToArray());
        }

        // Assert
        Assert.Contains("\"id\"", json);
        Assert.Contains("\"name\"", json);
        Assert.Contains("\"content\"", json);
        Assert.Contains("\"courseId\"", json);
        Assert.DoesNotContain("\"professorId\"", json);
        Assert.DoesNotContain("\"professor\"", json);
        Assert.DoesNotContain("\"studentCourses\"", json);
    }
}
