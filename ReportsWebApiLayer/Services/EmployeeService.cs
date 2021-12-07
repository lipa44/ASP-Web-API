using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using ReportsLibrary.Employees;
using ReportsLibrary.Entities;
using ReportsWebApiLayer.DataBase;
using ReportsWebApiLayer.Services.Interfaces;

namespace ReportsWebApiLayer.Services;

public class EmployeeService : IEmployeeService
{
    private readonly ReportsDbContext _dbContext;
    
    public EmployeeService(ReportsDbContext context)
    {
        _dbContext = context;
    }
    
    public ActionResult<List<Employee>> GetEmployees() => _dbContext.Employees.ToList();

    public async Task<Employee> RegisterEmployee(Employee employee)
    {
        ArgumentNullException.ThrowIfNull(employee);

        if (IsEmployeeExist(employee.Id).Result)
            throw new Exception($"Employee {employee} to add is already exist");

        EntityEntry<Employee> newEmployee = await _dbContext.Employees.AddAsync(employee);
        await _dbContext.SaveChangesAsync();

        return newEmployee.Entity;
    }

    public async void RemoveEmployee(Employee employee)
    {
        ArgumentNullException.ThrowIfNull(employee);

        if (!IsEmployeeExist(employee.Id).Result)
            throw new Exception($"Employee {employee} to remove doesn't exist");

        _dbContext.Employees.Remove(employee);
        await _dbContext.SaveChangesAsync();
    }

    public Employee GetEmployeeById(Guid id)
    {
        if (!IsEmployeeExist(id).Result)
            throw new Exception("Employee to get doesn't exist");

        return _dbContext.Employees.Single(e => e.Id == id);
    }

    public Employee? FindEmployeeById(Guid id)
    {
        return _dbContext.Employees.SingleOrDefault(e => e.Id == id);
    }

    private async Task<bool> IsEmployeeExist(Guid id) 
        => await _dbContext.Employees.AnyAsync(e => e.Id == id);
}