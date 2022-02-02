namespace WebApi.DataTransferObjects;

public record ReportsTaskDto
{
    public string Title { get; init; }
    public string Content { get; init; }
    public string State { get; init; }
}