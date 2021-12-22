namespace ReportsWebApi.DataTransferObjects;

public class FullReportDto
{
    public string OwnerData { get; init; }
    public string WorkTeamName { get; init; }
    public IReadOnlyCollection<TaskModificationDto> Modifications { get; init; }
}