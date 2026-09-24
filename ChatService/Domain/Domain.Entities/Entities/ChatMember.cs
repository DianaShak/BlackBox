using Domain.Entities.Enums;

namespace Domain.Entities.Entities;

public class ChatMember
{
    public Guid ChatMemberId { get; }

    public Guid ChatId { get; }

    public Guid UserId { get; }

    public MemberRole Role { get; }

    public DateTime JoinedAt { get; }
}
