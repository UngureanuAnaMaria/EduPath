using Application.DTOs;
using Domain.Common;
using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class StudentRepository : IStudentRepository
    {
        private readonly ApplicationDbContext context;

        public StudentRepository(ApplicationDbContext context)
        {
            this.context = context;
        }
        public async Task<Result<Guid>> AddStudentAsync(Student student)
        {
            try
            {
                await context.Students.AddAsync(student);
                await context.SaveChangesAsync();
                return Result<Guid>.Success(student.Id);
            }
            catch (Exception ex)
            {
                return Result<Guid>.Failure(ex.InnerException!.ToString());
            }
        }

        public async Task DeleteStudentAsync(Guid id)
        {
            var student = context.Students.FirstOrDefault(x => x.Id == id);
            if (student != null)
            {
                context.Students.Remove(student);
                await context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Student>> GetAllStudentsAsync()
        {
            return await context.Students
                .Include(s => s.StudentCourses)
                .ToListAsync();
        }


        public async Task<Student> GetStudentByIdAsync(Guid id)
        {
            return await context.Students
                       .Include(s => s.StudentCourses)
                       .FirstOrDefaultAsync(s => s.Id == id);
        }


        public async Task<Result<Guid>> UpdateStudentAsync(Student student)
        {
            try
            {
                context.Entry(student).State = EntityState.Modified;

                if (student.StudentCourses != null)
                {
                    foreach (var studentCourse in student.StudentCourses)
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
                return Result<Guid>.Success(student.Id);
            }
            catch (Exception ex)
            {
                return Result<Guid>.Failure(ex.InnerException!.ToString());
            }
        }

        public async Task<(IEnumerable<Student> Students, int TotalCount)> GetFilteredStudentsAsync(string? name, bool? status, int pageNumber, int pageSize)
        {
            var query = context.Students.AsQueryable();

            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(s => s.Name.Contains(name));
            }

            if (status.HasValue)
            {
                query = query.Where(s => s.Status == status.Value);
            }

            var totalCount = await query.CountAsync();

            var students = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (students, totalCount);
        }

    }
}
