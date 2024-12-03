using Domain.Common;
using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class ProfessorRepository : IProfessorRepository
    {
        private readonly ApplicationDbContext context;

        public ProfessorRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<Result<Guid>> AddProfessorAsync(Professor professor)
        {
            try
            {
                await context.Professors.AddAsync(professor);
                await context.SaveChangesAsync();
                return Result<Guid>.Success(professor.Id);
            }
            catch (Exception ex)
            {
                return Result<Guid>.Failure(ex.InnerException!.ToString());
            }
        }

        public async Task DeleteProfessorAsync(Guid id)
        {
            var professor = await context.Professors.FirstOrDefaultAsync(x => x.Id == id);
            if (professor != null)
            {
                context.Professors.Remove(professor);
                await context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Professor>> GetAllProfessorsAsync()
        {
            return await context.Professors.ToListAsync();
        }

        public async Task<Professor> GetProfessorByIdAsync(Guid id)
        {
            return await context.Professors.FindAsync(id);
        }

        public async Task<Result<Guid>> UpdateProfessorAsync(Professor professor)
        {
            try
            {
                context.Entry(professor).State = EntityState.Modified;
                await context.SaveChangesAsync();
                return Result<Guid>.Success(professor.Id);
            }
            catch (Exception ex)
            {
                return Result<Guid>.Failure(ex.InnerException!.ToString());
            }
        }

        public async Task<(IEnumerable<Professor> Professors, int TotalCount)> GetFilteredProfessorsAsync(string? name, bool? status, int pageNumber, int pageSize)
        {
            var query = context.Professors.AsQueryable();

            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(p => p.Name.Contains(name));
            }

            if (status.HasValue)
            {
                query = query.Where(p => p.Status == status.Value);
            }

            var totalCount = await query.CountAsync();

            var professors = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (professors, totalCount);
        }
    }
}
