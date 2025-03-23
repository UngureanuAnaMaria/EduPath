using Domain.Common;
using Domain.Entities;

namespace Domain.Repositories
{
    public interface IUserRepository
    {
        Task<Guid> Register(User user, CancellationToken cancellationToken);
        Task<LoginResult> Login(User user);
        Task<List<User>> GetAllUsersAsync();
    }
}
