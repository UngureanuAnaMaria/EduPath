using Domain.Common;
using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class StudentCourseRepository : IStudentCourseRepository
    {
        private readonly ApplicationDbContext context;
        public StudentCourseRepository(ApplicationDbContext context)
        {
            this.context = context;
        }
        public async Task<Result<Guid>> AddStudentCourseAsync(StudentCourse studentCourse)
        {
            try
            {
                await context.StudentCourses.AddAsync(studentCourse);
                await context.SaveChangesAsync();
                return Result<Guid>.Success(studentCourse.Id);
            }
            catch (Exception ex)
            {
                return Result<Guid>.Failure(ex.InnerException!.ToString());
            }
        }

        public async Task<IEnumerable<StudentCourse>> GetAllStudentCoursesAsync()
        {
            return await context.StudentCourses.ToListAsync();
        }


        public async Task<(IEnumerable<StudentCourse> StudentCourses, int TotalCount)> GetFilteredStudentCoursesAsync(Guid? studentId, Guid? courseId, int pageNumber, int pageSize)
        {
            var query = context.StudentCourses.AsQueryable();

            if (studentId.HasValue)
            {
                query = query.Where(sc => sc.StudentId == studentId.Value);
            }

            if (courseId.HasValue)
            {
                query = query.Where(sc => sc.CourseId == courseId.Value);
            }

            var totalCount = await query.CountAsync();

            var studentCourses = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (studentCourses, totalCount);
        }

    }
}
