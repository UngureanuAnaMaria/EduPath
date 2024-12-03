using Domain.Common;
using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class CourseRepository : ICourseRepository
    {
        private readonly ApplicationDbContext context;

        public CourseRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<Result<Guid>> AddCourseAsync(Course course)
        {
            try
            {
                await context.Courses.AddAsync(course);
                await context.SaveChangesAsync();
                return Result<Guid>.Success(course.Id);
            }
            catch (Exception ex)
            {
                return Result<Guid>.Failure(ex.InnerException!.ToString());
            }
        }

        public async Task DeleteCourseAsync(Guid id)
        {
            var course = await context.Courses.FirstOrDefaultAsync(x => x.Id == id);
            if (course != null)
            {
                context.Courses.Remove(course);
                await context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Course>> GetAllCoursesAsync()
        {
            return await context.Courses.ToListAsync();
        }

        public async Task<Course> GetCourseByIdAsync(Guid id)
        {
            return await context.Courses.FindAsync(id);
        }

        public async Task<Result<Guid>> UpdateCourseAsync(Course course)
        {
            try
            {
                context.Entry(course).State = EntityState.Modified;

                if (course.StudentCourses != null)
                {
                    foreach (var studentCourse in course.StudentCourses)
                    {
                        var existingStudentCourse = await context.StudentCourses
                            .FirstOrDefaultAsync(sc => sc.StudentId == studentCourse.StudentId && sc.CourseId == studentCourse.CourseId);

                        if (existingStudentCourse == null)
                        {
                            await context.StudentCourses.AddAsync(studentCourse);
                        }
                        else
                        {
                            context.Entry(existingStudentCourse).CurrentValues.SetValues(studentCourse);
                        }
                    }
                }

                await context.SaveChangesAsync();
                return Result<Guid>.Success(course.Id);
            }
            catch (Exception ex)
            {
                return Result<Guid>.Failure(ex.InnerException!.ToString());
            }
        }

        public async Task<(IEnumerable<Course> Courses, int TotalCount)> GetFilteredCoursesAsync(string? name, Guid? professorId, int pageNumber, int pageSize)
        {
            var query = context.Courses.AsQueryable();

            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(c => c.Name.Contains(name));
            }

            if (professorId.HasValue)
            {
                query = query.Where(c => c.ProfessorId == professorId.Value);
            }

            var totalCount = await query.CountAsync();

            var courses = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (courses, totalCount);
        }

    }
}