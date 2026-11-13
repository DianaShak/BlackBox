namespace Services.Contracts.Chat;

public class CreatingChatDto
{
    /// <summary>
    /// Название.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Идентификатор владельца чата.
    /// </summary>
    public Guid OwnerId { get; set; }

    /// <summary>
    /// Дата создания.
    /// </summary>
    public DateTime CreatedAt { get; set; }
}
