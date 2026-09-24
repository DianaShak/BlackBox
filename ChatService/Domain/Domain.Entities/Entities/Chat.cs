namespace Domain.Entities.Entities;

public class Chat
{
    public Guid ChatId { get; }

    public string Name { get; }

    public Guid OwnerId { get; }

    public DateTime CreatedAt { get; }

    public List<ChatMember> Members { get; }

    public List<Message> Messages { get; }
}
