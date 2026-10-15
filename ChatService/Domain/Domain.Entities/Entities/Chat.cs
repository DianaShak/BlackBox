namespace Domain.Entities.Entities;

public class Chat : IEntity<Guid>
{
    public Guid Id { get; set; }

    public string? Name { get; set; }

    public Guid OwnerId { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual ICollection<ChatMember>? ChatMembers { get; set; }

    public virtual ICollection<Message>? Messages { get; set; }
}
