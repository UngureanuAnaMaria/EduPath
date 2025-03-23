using Domain.Common;
using Domain.Entities;
using Domain.Repositories;
using MediatR;

public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, LoginResult>
{
    private readonly IUserRepository userRepository;

    public LoginUserCommandHandler(IUserRepository userRepository)
    {
        this.userRepository = userRepository;
    }

    public async Task<LoginResult> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        var user = new User
        {
            Email = request.Email,
            PasswordHash = request.Password
        };
        var loginResult = await userRepository.Login(user);
        return loginResult;
    }
}