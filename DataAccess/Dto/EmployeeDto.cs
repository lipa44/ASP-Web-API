namespace DataAccess.Dto;

public record EmployeeDto
{
    public string Name { get; init; }
    public string Surname { get; init; }
}