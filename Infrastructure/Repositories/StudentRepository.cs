using Application.AI_ML_Module;
using Application.DTOs;
using Domain.Common;
using Domain.Entities;
using Domain.Predictions;
using Domain.Repositories;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.ML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class StudentRepository : IStudentRepository
    {
        private readonly ApplicationDbContext context;
        private readonly StudentPerformancePredictionModel studentPerformancePredictionModel;
        private readonly string averageGradeModelPath= "C:\\Users\\Tallya\\Desktop\\EduPath\\EduPath\\averageGradeModel.zip";
        private readonly string percentageCompletedCoursesModelPath = "C:\\Users\\Tallya\\Desktop\\EduPath\\EduPath\\percentageCompletedCoursesModel.zip";
        private readonly string learningPathModelPath = "C:\\Users\\Tallya\\Desktop\\EduPath\\EduPath\\learningPathModel.zip";
        private readonly string futureCareerModelPath = "C:\\Users\\Tallya\\Desktop\\EduPath\\EduPath\\futureCareerModel.zip";
        private readonly string csvFilePath = "C:\\Users\\Tallya\\Desktop\\EduPath\\EduPath\\cs_students.csv";
        public StudentRepository(ApplicationDbContext context)
        
        {
            this.context = context;
            this.studentPerformancePredictionModel = new StudentPerformancePredictionModel();

            studentPerformancePredictionModel.LoadAverageGradeModel(averageGradeModelPath);
            studentPerformancePredictionModel.LoadPercentageCompletedCoursesModel(percentageCompletedCoursesModelPath);
            studentPerformancePredictionModel.LoadLearningPathModel(learningPathModelPath);
            studentPerformancePredictionModel.LoadFutureCareerPathModel(futureCareerModelPath);
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
            var query = context.Students.Include(s => s.StudentCourses).AsQueryable();

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

        public async Task<Domain.Predictions.StudentPredictions> GetPredictionForStudentAsync(Guid studentId)
        {
            var student = await context.Students.Include(s => s.StudentCourses).FirstOrDefaultAsync(s => s.Id == studentId);

            if (student == null)
            {
                throw new InvalidOperationException("Student not found.");
            }

            var studentData = new StudentData
            {
                Name = student.Name,
                Email = student.Email,
                Password = student.Password,
                Status = student.Status,
                CreatedAt = student.CreatedAt,
                LastLogin = student.LastLogin ?? DateTime.MinValue,
                AverageGrade = 0, 
                PercentageCompletedCourses = 0, 
                LearningPath = "string"
            };

            var predictions = new Domain.Predictions.StudentPredictions
            {
                AverageGrade = studentPerformancePredictionModel.PredictAverageGrade(studentData),
                PercentageCompletedCourses = studentPerformancePredictionModel.PredictPercentageCompletedCourses(studentData),
                LearningPath = studentPerformancePredictionModel.PredictLearningPath(studentData)
            };

            return predictions;
        }

        public async Task<StudentPredictionsExtern> PostPredictionForStudentExternAsync(StudentPredictionDatas student)
        {
            var validLevels = new[] { "Weak", "Strong", "Average" };
            var validGenders = new[] { "Male", "Female" };

            if(!validGenders.Contains(student.Gender, StringComparer.OrdinalIgnoreCase))
            {
                throw new ArgumentException("Invalid value for Gender. Allowed values are: Male, Female.");
            }    

            if (!validLevels.Contains(student.Python, StringComparer.OrdinalIgnoreCase))
            {
                throw new ArgumentException("Invalid value for Python. Allowed values are: Weak, Strong, Average.");
            }

            if (!validLevels.Contains(student.SQL, StringComparer.OrdinalIgnoreCase))
            {
                throw new ArgumentException("Invalid value for SQL. Allowed values are: Weak, Strong, Average.");
            }

            if (!validLevels.Contains(student.Java, StringComparer.OrdinalIgnoreCase))
            {
                throw new ArgumentException("Invalid value for Java. Allowed values are: Weak, Strong, Average.");
            }

            var studentDataExtern = new StudentDataExtern
            {
                Name = student.Name,
                Gender = student.Gender,
                Age = student.Age,
                GPA = student.GPA,
                Major = student.Major,
                InterestedDomain = student.InterestedDomain,
                Projects = student.Projects,
                Python = student.Python,
                SQL = student.SQL,
                Java = student.Java,

            };

            //studentPerformancePredictionModel.TrainFutureCareerModel(csvFilePath, futureCareerModelPath);

            var predictions = new StudentPredictionsExtern
            {
                FutureCareer = studentPerformancePredictionModel.PredictFutureCareer(studentDataExtern)
            };

            return predictions;
        }
    }
}
