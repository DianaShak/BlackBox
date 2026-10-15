using Domain.Entities.Enums;

namespace Services.Contracts.Member;

public class ChatMemberDto
{
    /// <summary>
    /// Идентификатор участника чата.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Идентификатор пользователя.
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Роль участника чата.
    /// </summary>
    public MemberRole Role { get; set; }
}
