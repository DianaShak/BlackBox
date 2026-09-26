using System.Runtime.CompilerServices;

namespace UserService.Domain.Entities;

public class UserProfile
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public string DisplayName { get; set; }

    public string? Bio { get; set; }

    public DateOnly? BirthDate { get; set; }

    public DateTime CreatedAt { get; set; }

    internal static UserProfile CreateDefault(Guid userId, string displayName)
    {
        if (string.IsNullOrWhiteSpace(displayName))
        {
            throw new ArgumentNullException("Имя пользователя не может быть пустым.");
        }

        if (displayName.Length < 2)
        {
            throw new ArgumentException("Имя пользователя должно быть не менее 2 символов.");
        }

        return new UserProfile
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            DisplayName = displayName,
            Bio = string.Empty,
            CreatedAt = DateTime.UtcNow
        };
    }

    public void Update(string displayName, string bio, DateOnly? birthDate)
    {
        if (string.IsNullOrWhiteSpace(displayName))
        {
            throw new ArgumentNullException("Имя не может быть пустым.");
        }

        if (birthDate.HasValue && birthDate.Value > DateOnly.FromDateTime(DateTime.Today))
        {
            throw new ArgumentException("Дата рождения не может быть в будущем.");
        }

        this.DisplayName = displayName;
        this.Bio = bio;
        this.BirthDate = birthDate;
    }
}