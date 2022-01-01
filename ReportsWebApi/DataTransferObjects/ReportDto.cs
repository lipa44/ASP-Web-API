namespace ReportsWebApi.DataTransferObjects;

public record ReportDto
{
    public string OwnerData { get; init; }
    public string WorkTeamName { get; init; }
}