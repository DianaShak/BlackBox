namespace Services.Contracts.Messages;

public class CreatingMessageDto
{
    public Guid Id { get; set; }

    public Guid ChatId { get; set; }

    public Guid AuthorId { get; set; }

    public string? Content { get; set; }

    public DateTime SentAt { get; set; }
}
