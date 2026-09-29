using Domain.Entities.Enums;

namespace Domain.Entities.Entities;

public class User : IEntity<Guid>
{
    public Guid Id { get; set; }

    public string Login { get; set; }

    public string PasswordHash { get; set; }

    public string Status { get; set; }

    public MemberRole UserRole { get; set; }
}
