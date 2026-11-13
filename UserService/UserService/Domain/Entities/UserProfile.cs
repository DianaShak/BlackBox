using System.Runtime.CompilerServices;

namespace UserService.Domain.Entities;

public class UserProfile
{
    public Guid Id { get; private set; }

    public Guid UserId { get; private set; }

    public string? Bio { get; private set; }

    public DateOnly? BirthDate { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public UserProfile(Guid id, Guid userId, string? bio, DateOnly? birthDate, DateTime createdAt)
    {
        Id = id;
        UserId = userId;
        Bio = bio;
        BirthDate = birthDate;
        CreatedAt = createdAt;
    }

    public static UserProfile CreateDefault(Guid userId)
    {
        return new UserProfile(Guid.NewGuid(), userId, string.Empty, null, DateTime.UtcNow);
    }

    public void Update(string bio, DateOnly? birthDate)
    {
        if (birthDate.HasValue && birthDate.Value > DateOnly.FromDateTime(DateTime.Today))
        {
            throw new ArgumentException("Дата рождения не может быть в будущем.");
        }

        this.Bio = bio;
        this.BirthDate = birthDate;
    }
}