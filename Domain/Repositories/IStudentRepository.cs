using Domain.Common;
using Domain.Entities;


namespace Domain.Repositories
{
    public interface IStudentRepository
    {
        Task<IEnumerable<Student>> GetAllStudentsAsync();
        Task<Student> GetStudentByIdAsync(Guid Id);
        Task<Result<Guid>> AddStudentAsync(Student student);
        Task DeleteStudentAsync(Guid Id);
        Task<Result<Guid>> UpdateStudentAsync(Student student);
        Task<(IEnumerable<Student> Students, int TotalCount)> GetFilteredStudentsAsync(string? name, bool? status, int pageNumber, int pageSize);
    }

}
