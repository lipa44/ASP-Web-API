namespace ReportsInfrastructure.Services.Interfaces;

using ReportsDomain.Entities;
using ReportsDomain.Enums;

public interface IEmployeesService
{
    Task<List<Employee>> GetEmployees();
    Task<Employee> GetEmployeeById(Guid employeeId);
    Task<Employee> FindEmployeeById(Guid employeeId);
    Task<Employee> CreateEmployee(Guid employeeId, string name, string surname, EmployeeRoles role);
    Task<Employee> SetChief(Guid employeeId, Guid chiefId);
    Task<Employee> RemoveEmployee(Guid employeeId);
    Task<List<Employee>> GetEmployeeSubordinatesById(Guid employeeId);
}