using Domain.Common;
using Domain.Entities;

namespace Domain.Repositories
{
    public interface ILessonRepository
    {
        Task<IEnumerable<Lesson>> GetAllLessonsAsync();
        Task<Lesson> GetLessonByIdAsync(Guid id);
        Task<Result<Guid>> AddLessonAsync(Lesson lesson);
        Task DeleteLessonAsync(Guid id);
        Task<Result<Guid>> UpdateLessonAsync(Lesson lesson);
        Task<(IEnumerable<Lesson> Lessons, int TotalCount)> GetFilteredLessonsAsync(string? name, Guid? courseId, int pageNumber, int pageSize);
    }

}
