using Services.Contracts.Member;

namespace Services.Contracts.Chat;

public class UpdatingChatWithMembersDto
{
    /// <summary>
    /// Название.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Участники чата.
    /// </summary>
    public IEnumerable<AttachingChatMembersDto>? ChatMembers { get; set; }
}
