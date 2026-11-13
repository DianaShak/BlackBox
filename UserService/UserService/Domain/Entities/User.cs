using System.Reflection;
using System.Runtime.CompilerServices;
using UserService.Domain.Enums;
using UserService.Domain.Interfaces;
using UserService.Domain.Value_Objects;

namespace UserService.Domain.Entities;

public class User
{
    public Guid Id { get; private set; }
    
    public string Login { get; private set; }
    
    public string PasswordHash { get; private set; }
    
    public string DisplayName { get; private set; }
    
    public UserStatusEnum Status { get; private set; }
    
    public UserRole UserRole { get; private set; }

    public List<PasswordRecord> PasswordHistory { get; private set; } = new();
    
    [Assembly:InternalsVisibleTo("UserService.Infrastructure.Mappings.UserMapping")]
    internal User(Guid id, string login, string passwordHash, string displayName, UserStatusEnum status, UserRole role, List<PasswordRecord> passwordHistory)
    {
        Id = id;
        Login = login;
        PasswordHash = passwordHash;
        DisplayName = displayName;
        Status = status;
        UserRole = role;
        PasswordHistory = passwordHistory;
    }
    
    public static User Register(string login, string password, string displayName, IPasswordHasher passwordHasher)
    {
        if (string.IsNullOrWhiteSpace(login))
        {
            throw new ArgumentNullException("Логин пользователя не может быть пустым.");
        }

        if (login.Length < 2)
        {
            throw new ArgumentException("Логин пользователя должен быть не менее 2 символов.");
        }

        if (string.IsNullOrWhiteSpace(displayName))
        {
            throw new ArgumentNullException("Имя пользователя не может быть пустым.");
        }

        if (displayName.Length < 2)
        {
            throw new ArgumentException("Имя пользователя должно быть не менее 2 символов.");
        }

        if (password.Length < 8) throw new Exception("Пароль слишком короткий");
        
        var userId = Guid.NewGuid();
        var passwordHistory = new List<PasswordRecord>();
        var hashedPassword = passwordHasher.HashPassword(password);
        
        var user = new User(
            userId,
            login,
            hashedPassword,
            displayName,
            UserStatusEnum.Offline,
            UserRole.Member,
            passwordHistory
            );

        user.PasswordHistory.Add(new PasswordRecord(hashedPassword));

        return user;
    }

    public void ChangePassword(string newPasswordHash)
    {
        if (string.IsNullOrWhiteSpace(newPasswordHash))
        {
            throw new ArgumentNullException("Новый пароль не может быть пустым.");
        }

        if (PasswordHistory.Any(x => x.PasswordHash == newPasswordHash))
        {
            throw new InvalidOperationException("Новый пароль должен отличаться от предыдущих.");
        }
        
        this.PasswordHash = newPasswordHash;
        this.PasswordHistory.Add(new PasswordRecord(newPasswordHash));
    }
    
    public void LogOut()
    {
        Status = UserStatusEnum.Offline;
        UserRole = UserRole.Guest;
    }
}