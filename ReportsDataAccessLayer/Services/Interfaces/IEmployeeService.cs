using ReportsLibrary.Employees;
using ReportsLibrary.Tools;

namespace ReportsDataAccessLayer.Services.Interfaces
{
    public interface IEmployeeService
    {
        Task<Employee> RegisterEmployee(Guid employeeToRegisterId, string name, string surname, EmployeeRoles role);
        Task<Employee> SetChief(Guid employeeId, Guid chiefId);
        Task<Employee> SetWorkTeam(Guid employeeId, Guid changerId, Guid workTeamId);
        Task<Employee> RemoveWorkTeam(Guid employeeId, Guid changerId, Guid workTeamId);
        void RemoveEmployee(Guid employeeId);
        void CommitChangesToReport(Guid employeeId);
        Task<Employee> GetEmployeeByIdAsync(Guid id);
        Task<Employee> FindEmployeeByIdAsync(Guid id);
        Task<List<Employee>> GetEmployees();
    }
}