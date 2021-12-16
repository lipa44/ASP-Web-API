namespace ReportsWebApiLayer.DataTransferObjects;

public class EmployeeDto
{
    public EmployeeDto() { }

    public Guid Id { get; init; }
    public string Name { get; init; }
    public string Surname { get; init; }
    public Guid? WorkTeamId { get; init; }
    public Guid? ChiefId { get; init; }
}