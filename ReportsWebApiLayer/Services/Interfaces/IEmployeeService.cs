using Microsoft.AspNetCore.Mvc;
using ReportsLibrary.Employees;

namespace ReportsWebApiLayer.Services.Interfaces
{
    public interface IEmployeeService
    {
        Task<Employee> RegisterEmployee(Employee employee);
        Task<Employee> SetChief(Employee employee, Employee chief);
        void RemoveEmployee(Employee employee);
        Task<Employee> GetEmployeeById(Guid id);
        Task<Employee> FindEmployeeById(Guid id);
        ActionResult<List<Employee>> GetEmployees();
    }
}