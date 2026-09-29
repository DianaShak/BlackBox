namespace Domain.Entities.Entities;

public class Message : IEntity<Guid>
{
    public const int MaxContentLength = 2000;

    public Guid Id { get; set; }

    public Guid ChatId { get; set; }

    public Guid AuthorId { get; set; }

    public string Content { get; set; }

    public DateTime SentAt { get; set; }
}
