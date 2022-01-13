namespace Presentation.DataTransferObjects;

public record EmployeeDto
{
    public string Name { get; init; }
    public string Surname { get; init; }
}