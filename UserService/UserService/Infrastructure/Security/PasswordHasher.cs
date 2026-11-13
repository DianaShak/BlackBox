using BCrypt.Net;
using UserService.Domain.Interfaces;

namespace UserService.Infrastructure.Security;

using BCrypt.Net;

public class PasswordHasher : IPasswordHasher
{
    public string HashPassword(string password)
    {
        return BCrypt.EnhancedHashPassword(password, workFactor: 11);
    }

    public bool VerifyPassword(string password, string passwordHash)
    {
        return BCrypt.EnhancedVerify(password, passwordHash);
    }
}