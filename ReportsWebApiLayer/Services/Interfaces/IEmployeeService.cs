using Microsoft.AspNetCore.Mvc;
using ReportsLibrary.Employees;

namespace ReportsWebApiLayer.Services.Interfaces
{
    public interface IEmployeeService
    {
        Task<Employee> RegisterEmployee(Employee employee);
        void RemoveEmployee(Employee employee);
        Employee GetEmployeeById(Guid id);
        Employee? FindEmployeeById(Guid id);
        ActionResult<List<Employee>> GetEmployees();
    }
}