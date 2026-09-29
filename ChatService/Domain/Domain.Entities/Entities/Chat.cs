namespace Domain.Entities.Entities;

public class Chat : IEntity<Guid>
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public Guid OwnerId { get; set; }

    public DateTime CreatedAt { get; set; }

    public List<ChatMember> Members { get; set; }

    public List<Message> Messages { get; set; }
}
