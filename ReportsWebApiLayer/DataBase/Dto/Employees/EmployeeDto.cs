using ReportsLibrary.Employees;

namespace ReportsWebApiLayer.DataBase.Dto.Employees;

public abstract class EmployeeDto
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public Guid Id { get; set; }
    public Employee? Chief { get; set; }
    public List<Employee> Employees { get; set; }
}