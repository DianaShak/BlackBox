using UserService.Domain.Value_Objects;

namespace UserService.Domain.Entities;

public class User
{
    public Guid Id { get; private set; }
    
    public string Login { get; private set; }
    
    public string PasswordHash { get; private set; }
    
    public string Status { get; private set; }
    
    public UserRole UserRole { get; private set; }
    
    //  Навигационные свойства
    public UserProfile Profile { get; private set; }

    public List<PasswordRecord> PasswordHistory { get; private set; } = new();
    
    //  Для EF Core
    private User() { }

    public static User Register(string login, string passwordHash, string displayName)
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

        var userId = Guid.NewGuid();

        var user = new User
        {
            Id = userId,
            Login = login,
            PasswordHash = passwordHash,
            Status = "offline",
            UserRole = UserRole.Member,
        };

        user.Profile = UserProfile.CreateDefault(userId, displayName);
        user.PasswordHistory.Add(new PasswordRecord(passwordHash));

        return user;
    }
    //public bool VerifyPassword()
    //{
        
    //}
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
        Status = "offline";
        UserRole = UserRole.Guest;
    }
}