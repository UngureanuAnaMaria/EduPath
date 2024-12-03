using Domain.Common;
using Domain.Entities;

namespace Domain.Repositories
{
    public interface IStudentCourseRepository
    {
        Task<Result<Guid>> AddStudentCourseAsync(StudentCourse studentCourse);
        Task<IEnumerable<StudentCourse>> GetAllStudentCoursesAsync();
        Task<(IEnumerable<StudentCourse> StudentCourses, int TotalCount)> GetFilteredStudentCoursesAsync(Guid? studentId, Guid? courseId, int pageNumber, int pageSize);
    }

}
