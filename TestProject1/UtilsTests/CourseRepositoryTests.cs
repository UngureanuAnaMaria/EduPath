using System;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Persistence;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

public class CourseRepositoryTests
{
    private readonly ApplicationDbContext _context;
    private readonly ICourseRepository _courseRepository;

    public CourseRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("TestDatabase")
            .Options;

        _context = new ApplicationDbContext(options);
        _courseRepository = new CourseRepository(_context);
    }

    [Fact]
    public async Task AddCourseAsync_ShouldAddCourseSuccessfully()
    {
        // Arrange
        var course = new Course
        {
            Id = Guid.NewGuid(),
            Name = "Test Course",
            Description = "Test Description"
        };

        // Act
        var result = await _courseRepository.AddCourseAsync(course);

        // Assert
        var savedCourse = await _context.Courses.FindAsync(course.Id);
        Assert.NotNull(savedCourse);
        Assert.Equal(course.Id, savedCourse?.Id);
        Assert.Equal(course.Name, savedCourse?.Name);
        Assert.Equal(course.Description, savedCourse?.Description);
        Assert.True(result.IsSuccess);
    }



    [Fact]
    public async Task DeleteCourseAsync_ShouldDeleteCourseSuccessfully()
    {
        // Arrange
        var course = new Course
        {
            Id = Guid.NewGuid(),
            Name = "Test Course to Delete",
            Description = "Test Description"
        };

        _context.Courses.Add(course);
        await _context.SaveChangesAsync();

        // Act
        await _courseRepository.DeleteCourseAsync(course.Id);

        // Assert
        var deletedCourse = await _context.Courses.FindAsync(course.Id);
        Assert.Null(deletedCourse);
    }


    [Fact]
    public async Task GetCourseByIdAsync_ShouldReturnCourse()
    {
        // Arrange
        var course = new Course { Id = Guid.NewGuid(), Name = "Course", Description = "Description" };
        _context.Courses.Add(course);
        await _context.SaveChangesAsync();

        // Act
        var result = await _courseRepository.GetCourseByIdAsync(course.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(course.Id, result.Id);
        Assert.Equal(course.Name, result.Name);
    }

    [Fact]
    public async Task GetCourseByIdAsync_ShouldReturnNullWhenCourseNotFound()
    {
        // Arrange
        var nonExistingCourseId = Guid.NewGuid();

        // Act
        var result = await _courseRepository.GetCourseByIdAsync(nonExistingCourseId);

        // Assert
        Assert.Null(result);
    }



    [Fact]
    public async Task UpdateCourseAsync_ShouldUpdateCourseSuccessfully()
    {
        // Arrange
        var course = new Course { Id = Guid.NewGuid(), Name = "Old Course", Description = "Old Description" };
        _context.Courses.Add(course);
        await _context.SaveChangesAsync();

        course.Name = "Updated Course";
        course.Description = "Updated Description";

        // Act
        var result = await _courseRepository.UpdateCourseAsync(course);

        // Assert
        var updatedCourse = await _context.Courses.FindAsync(course.Id);
        Assert.NotNull(updatedCourse);
        Assert.Equal(course.Name, updatedCourse?.Name);
        Assert.Equal(course.Description, updatedCourse?.Description);
        Assert.True(result.IsSuccess);
    }



    [Fact]
    public async Task GetFilteredCoursesAsync_ShouldReturnEmptyWhenNoMatch()
    {
        // Arrange
        var course1 = new Course { Id = Guid.NewGuid(), Name = "Math Course", Description = "Math Course" };

        _context.Courses.Add(course1);
        await _context.SaveChangesAsync();

        // Act
        var result = await _courseRepository.GetFilteredCoursesAsync("Science", null, 1, 10);

        // Assert
        Assert.Empty(result.Courses);
    }


}


