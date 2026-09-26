namespace UserService.Domain.Value_Objects;

public record PasswordRecord
{
    public string PasswordHash { get; private set; }
    
    public DateTime ChangedAt { get; private set; }
    //  Конструктор для EF core
    private PasswordRecord() { }

    public PasswordRecord(string passwordHash)
    {
        if (string.IsNullOrWhiteSpace(passwordHash))
        {
            throw new ArgumentNullException("Хэш пароля не может быть пустым.");
        }
        
        PasswordHash = passwordHash;
        ChangedAt = DateTime.UtcNow;
    }
}