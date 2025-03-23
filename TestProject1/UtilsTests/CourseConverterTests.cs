using System;
using System.Collections.Generic;
using System.Text.Json;
using Application.DTOs;
using Application.Utils;
using Xunit;

public class CourseConverterTests
{
    [Fact]
    public void Write_CourseDTO_WritesExpectedJson()
    {
        // Arrange
        var courseDto = new CourseDTO
        {
            Id = Guid.NewGuid(),
            Name = "Mathematics 101",
            Description = "An introductory course to Mathematics",
            ProfessorId = Guid.NewGuid(),
            Professor = new ProfessorDTO
            {
                Id = Guid.NewGuid(),
                Name = "Dr. John Doe",
                Email = "johndoe@example.com",
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
                        Name = "Jane Doe",
                        Email = "janedoe@example.com",
                        Password = "password",
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
                    Name = "Introduction to Algebra",
                    Content = "Basic Algebra concepts",
                    CourseId = Guid.NewGuid()
                }
            }
        };

        var jsonConverter = new CourseConverter();
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
                jsonConverter.Write(writer, courseDto, options);
            }
            json = System.Text.Encoding.UTF8.GetString(stream.ToArray());
        }

        // Assert
        Assert.Contains("\"id\"", json);
        Assert.Contains("\"name\"", json);
        Assert.Contains("\"description\"", json);
        Assert.Contains("\"professorId\"", json);
        Assert.Contains("\"professor\"", json);
        Assert.Contains("\"studentCourses\"", json);
        Assert.Contains("\"lessons\"", json);
    }

    [Fact]
    public void Write_CourseDTO_WithNullValues_ExcludesNullProperties()
    {
        // Arrange
        var courseDto = new CourseDTO
        {
            Id = Guid.NewGuid(),
            Name = "Physics 101",
            Description = "An introductory course to Physics",
            ProfessorId = null,  // Null value for professor
            Professor = null,  // Null professor
            StudentCourses = null,  // No student courses
            Lessons = null  // No lessons
        };

        var jsonConverter = new CourseConverter();
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
                jsonConverter.Write(writer, courseDto, options);
            }
            json = System.Text.Encoding.UTF8.GetString(stream.ToArray());
        }

        // Assert
        Assert.Contains("\"id\"", json);
        Assert.Contains("\"name\"", json);
        Assert.Contains("\"description\"", json);
        Assert.DoesNotContain("\"professorId\"", json);
        Assert.DoesNotContain("\"professor\"", json);
        Assert.DoesNotContain("\"studentCourses\"", json);
        Assert.DoesNotContain("\"lessons\"", json);
    }
}
