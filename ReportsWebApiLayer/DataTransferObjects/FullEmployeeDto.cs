namespace ReportsWebApiLayer.DataTransferObjects;

public class FullEmployeeDto
{
    public Guid Id { get; init; }
    public string Name { get; init; }
    public string Surname { get; init; }
    public string Role { get; init; }
    public string ChiefData { get; init; }

    public IEnumerable<ReportsTaskDto> Tasks { get; init; }
    public IEnumerable<EmployeeDto> Subordinates { get; init; }
}