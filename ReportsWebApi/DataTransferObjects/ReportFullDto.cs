namespace ReportsWebApi.DataTransferObjects;

public record ReportFullDto
{
    public string OwnerData { get; init; }
    public string WorkTeamName { get; init; }
    public IReadOnlyCollection<TaskModificationDto> Modifications { get; init; }
}