using Domain.Common;
using Domain.Entities;

namespace Domain.Repositories
{
    public interface IProfessorRepository
    {
        Task<IEnumerable<Professor>> GetAllProfessorsAsync();
        Task<Professor> GetProfessorByIdAsync(Guid Id);
        Task<Result<Guid>> AddProfessorAsync(Professor professor);
        Task DeleteProfessorAsync(Guid Id);
        Task<Result<Guid>> UpdateProfessorAsync(Professor professor);
        Task<(IEnumerable<Professor> Professors, int TotalCount)> GetFilteredProfessorsAsync(string? name, bool? status, int pageNumber, int pageSize);
    }

}

