using System.Net;

namespace UserService.Application.Use_Cases.RegisterUser;

public class RegisterUserCommand
{
    public record RegisterUserCommand(string Login, string Password, string Username);
    public record RegisterUserCommandResponse(string Status);
}