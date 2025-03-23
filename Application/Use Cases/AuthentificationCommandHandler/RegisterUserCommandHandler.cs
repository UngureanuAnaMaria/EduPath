using Domain.Entities;
using Domain.Repositories;
using MediatR;

public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, Guid>
{
    private readonly IUserRepository repository;

    public RegisterUserCommandHandler(IUserRepository repository) => this.repository = repository;

    public async Task<Guid> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var user = new User
        {
            Email = request.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
            Admin = request.Admin
        };

        await repository.Register(user, cancellationToken);
        return user.Id;
    }
}
