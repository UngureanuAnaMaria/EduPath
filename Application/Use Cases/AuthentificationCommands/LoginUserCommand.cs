using Domain.Common;
using MediatR;

public class LoginUserCommand : IRequest<LoginResult>
{
    public string Email { get; set; }
    public string Password { get; set; }
    public string Role { get; set; }
}
