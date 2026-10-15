using Domain.Entities.Entities;

namespace Services.Contracts.Chat;

public class ChatDto
{
    public Guid Id { get; set; }

    public string? Name { get; set; }

    public Guid OwnerId { get; set; }

    public DateTime CreatedAt { get; set; }

    public List<ChatMember>? ChatMembers { get; set; }

    public List<Message>? Messages { get; set; }
}
