using Domain.Common;
using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class LessonRepository : ILessonRepository
    {
        private readonly ApplicationDbContext context;

        public LessonRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<Result<Guid>> AddLessonAsync(Lesson lesson)
        {
            try
            {
                await context.Lessons.AddAsync(lesson);
                await context.SaveChangesAsync();
                return Result<Guid>.Success(lesson.Id);
            }
            catch (Exception ex)
            {
                return Result<Guid>.Failure(ex.InnerException!.ToString());
            }
        }

        public async Task DeleteLessonAsync(Guid id)
        {
            var lesson = await context.Lessons.FirstOrDefaultAsync(x => x.Id == id);
            if (lesson != null)
            {
                context.Lessons.Remove(lesson);
                await context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Lesson>> GetAllLessonsAsync()
        {
            return await context.Lessons.ToListAsync();
        }

        public async Task<Lesson> GetLessonByIdAsync(Guid id)
        {
            return await context.Lessons.FindAsync(id);
        }

        public async Task<Result<Guid>> UpdateLessonAsync(Lesson lesson)
        {
            try
            {
                context.Entry(lesson).State = EntityState.Modified;
                await context.SaveChangesAsync();
                return Result<Guid>.Success(lesson.Id);
            }
            catch (Exception ex)
            {
                return Result<Guid>.Failure(ex.InnerException!.ToString());
            }
        }

        public async Task<(IEnumerable<Lesson> Lessons, int TotalCount)> GetFilteredLessonsAsync(string? name, Guid? courseId, int pageNumber, int pageSize)
        {
            var query = context.Lessons.AsQueryable();

            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(l => l.Name.Contains(name));
            }

            if (courseId.HasValue)
            {
                query = query.Where(l => l.CourseId == courseId.Value);
            }

            var totalCount = await query.CountAsync();

            var lessons = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (lessons, totalCount);
        }
        
    }
}
