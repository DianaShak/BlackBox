namespace Domain.Entities.Entities;

public class Message
{
    public const int MaxContentLength = 2000;

    public Guid MessageId { get; }

    public Guid ChatId { get; }

    public Guid AuthorId { get; }

    public string Content { get; }

    public DateTime SentAt { get; }
}
