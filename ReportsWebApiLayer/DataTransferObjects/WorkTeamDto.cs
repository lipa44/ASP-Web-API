namespace ReportsWebApiLayer.DataTransferObjects;

public class WorkTeamDto
{
    public WorkTeamDto() { }

    public string Name { get; init; }
    public Guid Id { get; init; }
    public Guid? TeamLeadId { get; init; }
    public Guid? ReportId { get; init; }
}