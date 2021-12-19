namespace ReportsWebApiLayer.DataTransferObjects;

public class FullWorkTeamDto
{
    public string Name { get; init; }
    public Guid Id { get; init; }
    public ReportDto Report { get; init; }
    public IEnumerable<SprintDto> Sprints { get; init; }
    public IEnumerable<EmployeeDto> Employees { get; init; }
}