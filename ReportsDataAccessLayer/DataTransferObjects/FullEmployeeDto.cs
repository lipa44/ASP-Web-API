namespace ReportsDataAccessLayer.DataTransferObjects;

public class FullEmployeeDto
{
    public Guid Id { get; init; }
    public string Name { get; init; }
    public string Surname { get; init; }
    public Guid? WorkTeamId { get; init; }
    public Guid? ChiefId { get; init; }

    public IEnumerable<ReportsTaskDto> Tasks { get; init; }
    public IEnumerable<EmployeeDto> Subordinates { get; init; }
}