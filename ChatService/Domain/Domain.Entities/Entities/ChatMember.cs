using Domain.Entities.Enums;

namespace Domain.Entities.Entities;

public class ChatMember : IEntity<Guid>
{
    public Guid Id { get; set; }

    public Guid ChatId { get; set; }

    public Guid UserId { get; set; }

    public MemberRole Role { get; set; }

    public DateTime JoinedAt { get; set; }
}
