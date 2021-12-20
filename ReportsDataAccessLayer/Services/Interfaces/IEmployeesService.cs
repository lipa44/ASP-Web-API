using ReportsLibrary.Employees;
using ReportsLibrary.Enums;

namespace ReportsDataAccessLayer.Services.Interfaces;

public interface IEmployeesService
{
    Task<Employee> RegisterEmployee(Guid employeeId, string name, string surname, EmployeeRoles role);
    Task<Employee> SetChief(Guid employeeId, Guid chiefId);
    Task<Employee> SetWorkTeam(Guid employeeId, Guid changerId, Guid workTeamId);
    Task<Employee> RemoveWorkTeam(Guid employeeId, Guid changerId, Guid workTeamId);
    Task<Employee> RemoveEmployee(Guid employeeId);
    Task<Employee> GetEmployeeByIdAsync(Guid employeeId);
    Task<Employee> FindEmployeeByIdAsync(Guid employeeId);
    Task<List<Employee>> GetEmployeesAsync();
}