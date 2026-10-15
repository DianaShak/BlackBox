namespace Services.Contracts.Chat;

public class ChatFilterDto
{
    public string? Name { get; set; }

    public int ItemsPerPage { get; set; }

    public int Page { get; set; }
}
