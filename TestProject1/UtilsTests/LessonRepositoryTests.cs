using Domain.Common;
using Domain.Entities;
using Infrastructure.Persistence;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Infrastructure.Tests.Repositories
{
    public class LessonRepositoryTests
    {
        private readonly ApplicationDbContext _context;
        private readonly LessonRepository _repository;

        public LessonRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _context = new ApplicationDbContext(options);
            _repository = new LessonRepository(_context);
        }

        [Fact]
        public async Task AddLessonAsync_ShouldAddLesson()
        {
            // Arrange
            var lesson = new Lesson
            {
                Id = Guid.NewGuid(),
                Name = "Test Lesson",
                CourseId = Guid.NewGuid(),
                Content = "Lesson content",
            };

            // Act
            var result = await _repository.AddLessonAsync(lesson);

            // Assert
            Assert.True(result.IsSuccess);
        
            var addedLesson = await _context.Lessons.FindAsync(lesson.Id);
            Assert.NotNull(addedLesson);
        }

        [Fact]
        public async Task GetLessonByIdAsync_ShouldReturnLesson()
        {
            // Arrange
            var lessonId = Guid.NewGuid();
            var lesson = new Lesson
            {
                Id = lessonId,
                Name = "Test Lesson",
                CourseId = Guid.NewGuid(),
                Content = "Lesson content",
            };
            await _context.Lessons.AddAsync(lesson);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetLessonByIdAsync(lessonId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(lessonId, result.Id);
        }

        [Fact]
        public async Task GetAllLessonsAsync_ShouldReturnLessons()
        {
            // Arrange
            var lesson1 = new Lesson
            {
                Id = Guid.NewGuid(),
                Name = "Test Lesson 1",
                CourseId = Guid.NewGuid(),
                Content = "Lesson content",
            };
            var lesson2 = new Lesson
            {
                Id = Guid.NewGuid(),
                Name = "Test Lesson 2",
                CourseId = Guid.NewGuid(),
                Content = "Lesson content",
            };
            await _context.Lessons.AddRangeAsync(lesson1, lesson2);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetAllLessonsAsync();

            // Assert
            Assert.NotEmpty(result);
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task DeleteLessonAsync_ShouldDeleteLesson()
        {
            // Arrange
            var lesson = new Lesson
            {
                Id = Guid.NewGuid(),
                Name = "Test Lesson",
                CourseId = Guid.NewGuid(),
                Content = "Lesson content",
            };
            await _context.Lessons.AddAsync(lesson);
            await _context.SaveChangesAsync();

            // Act
            await _repository.DeleteLessonAsync(lesson.Id);

            // Assert
            var deletedLesson = await _context.Lessons.FindAsync(lesson.Id);
            Assert.Null(deletedLesson);
        }

        [Fact]
        public async Task UpdateLessonAsync_ShouldUpdateLesson()
        {
            // Arrange
            var lesson = new Lesson
            {
                Id = Guid.NewGuid(),
                Name = "Test Lesson",
                CourseId = Guid.NewGuid(),
                Content = "Lesson content",
            };
            await _context.Lessons.AddAsync(lesson);
            await _context.SaveChangesAsync();

            lesson.Name = "Updated Test Lesson";
            lesson.Content = "Updated content";

            // Act
            var result = await _repository.UpdateLessonAsync(lesson);

            // Assert
            Assert.True(result.IsSuccess);
            var updatedLesson = await _context.Lessons.FindAsync(lesson.Id);
            Assert.Equal("Updated Test Lesson", updatedLesson.Name);
            Assert.Equal("Updated content", updatedLesson.Content);
        }

        [Fact]
        public async Task GetFilteredLessonsAsync_ShouldReturnFilteredLessons()
        {
            // Arrange
            var courseId = Guid.NewGuid();
            var lesson1 = new Lesson
            {
                Id = Guid.NewGuid(),
                Name = "Math Lesson 1",
                CourseId = courseId,
                Content = "Lesson content",
            };
            var lesson2 = new Lesson
            {
                Id = Guid.NewGuid(),
                Name = "Math Lesson 2",
                CourseId = courseId,
                Content = "Lesson content",
            };
            var lesson3 = new Lesson
            {
                Id = Guid.NewGuid(),
                Name = "Science Lesson",
                CourseId = Guid.NewGuid(),
                Content = "Lesson content",
            };
            await _context.Lessons.AddRangeAsync(lesson1, lesson2, lesson3);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetFilteredLessonsAsync("Math", courseId, 1, 10);

         
        }
    }
}
