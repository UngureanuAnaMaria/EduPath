using Domain.Common;
using Domain.Entities;

namespace Domain.Repositories
{
    public interface ICourseRepository
    {
        Task<IEnumerable<Course>> GetAllCoursesAsync();
        Task<Course> GetCourseByIdAsync(Guid id);
        Task<Result<Guid>> AddCourseAsync(Course course);
        Task DeleteCourseAsync(Guid id);
        Task<Result<Guid>> UpdateCourseAsync(Course course);
        Task<(IEnumerable<Course> Courses, int TotalCount)> GetFilteredCoursesAsync(string? name, Guid? professorId, int pageNumber, int pageSize);
    }

}
