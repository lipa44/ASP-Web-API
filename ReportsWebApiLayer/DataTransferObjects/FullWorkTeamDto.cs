namespace ReportsWebApiLayer.DataTransferObjects;

public class FullWorkTeamDto
{
    public string Name { get; init; }
    public Guid Id { get; init; }
    public IReadOnlyCollection<SprintDto> Sprints { get; init; }
    public IReadOnlyCollection<EmployeeDto> Employees { get; init; }
}