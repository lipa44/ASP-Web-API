using Domain.Entities;
using Domain.Enums;

namespace Services.Services.Interfaces;

public interface IEmployeesService
{
    Task<IReadOnlyCollection<Employee>> GetEmployees();
    Task<Employee> FindEmployeeById(Guid employeeId);
    Task<Employee> CreateEmployee(Guid employeeId, string name, string surname, EmployeeRoles role);
    Task<Report> CreateReport(Guid employeeId);
    Task<Employee> SetChief(Guid employeeId, Guid chiefId);
    Task<Employee> RemoveEmployee(Guid employeeId);
    Task<IReadOnlyCollection<Employee>> GetEmployeeSubordinatesById(Guid employeeId);
}