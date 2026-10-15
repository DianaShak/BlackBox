namespace Services.Contracts.Member;

public class CreatingChatMemberDto
{
    /// <summary>
    /// Идентификатор чата.
    /// </summary>
    public Guid ChatId { get; set; }

    /// <summary>
    /// Идентификатор пользователя.
    /// </summary>
    public Guid UserId { get; set; }
}
