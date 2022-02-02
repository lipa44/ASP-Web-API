namespace WebApi.DataTransferObjects;

public record SprintFullDto
{
    public DateTime ExpirationDate { get; init; }
    public Guid Id { get; init; }

    public IReadOnlyCollection<ReportsTaskDto> Tasks { get; init; }
}