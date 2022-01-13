namespace Presentation.DataTransferObjects;

public record ReportFullDto
{
    public string OwnerData { get; init; }
    public string State { get; init; }
    public IReadOnlyCollection<TaskModificationDto> Modifications { get; init; }
}