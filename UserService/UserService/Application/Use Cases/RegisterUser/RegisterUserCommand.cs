using System.Net;

namespace UserService.Application.Use_Cases.RegisterUser;

public record RegisterUserCommand(string Login, string Password, string Username);
