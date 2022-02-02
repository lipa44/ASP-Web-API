namespace WebApi.DataTransferObjects;

public record WorkTeamFullDto
{
    public string Name { get; init; }
    public string TeamLeadData { get; init; }
    public Guid Id { get; init; }
    public IReadOnlyCollection<SprintDto> Sprints { get; init; }
    public IReadOnlyCollection<EmployeeDto> Employees { get; init; }
}