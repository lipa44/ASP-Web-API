using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using ReportsLibrary.Employees;
using ReportsWebApiLayer.DataBase;
using ReportsWebApiLayer.Services.Interfaces;
using Task = ReportsLibrary.Tasks.Task;

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

        // EntityEntry<Task> newTask = await _dbContext.Tasks.AddAsync(new Task($"{employee}: aboba task"));
        //
        // newTask.Entity.SetImplementer(newEmployee.Entity, newEmployee.Entity);
        await _dbContext.SaveChangesAsync();

        return newEmployee.Entity;
    }

    public async Task<Employee> SetChief(Employee employee, Employee chief)
    {
        ArgumentNullException.ThrowIfNull(employee);
        ArgumentNullException.ThrowIfNull(chief);

        employee.SetChief(chief);

        await _dbContext.SaveChangesAsync();

        return employee;
    }

    public async void RemoveEmployee(Employee employee)
    {
        ArgumentNullException.ThrowIfNull(employee);

        if (!await IsEmployeeExist(employee.Id))
            throw new Exception($"Employee {employee} to remove doesn't exist");

        foreach (Task task in _dbContext.Tasks.Where(t => t.Id == employee.Id))
            _dbContext.Tasks.Remove(task);

        _dbContext.Remove(employee);

        // _dbContext.Employees.Remove(employee);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<Employee> GetEmployeeById(Guid id)
    {
        if (!IsEmployeeExist(id).Result)
            throw new Exception("Employee to get doesn't exist");

        return await _dbContext.Employees.SingleAsync(e => e.Id == id);
    }

    public async Task<Employee> FindEmployeeById(Guid id)
    {
        Employee employee = _dbContext.Employees.SingleOrDefaultAsync(e => e.Id == id).Result;

        // employee.AddSubordinate(new ("Isa", "Kudashev", Guid.NewGuid(), EmployeeRoles.TeamLead));
        await _dbContext.SaveChangesAsync();

        return employee;
    }

    private async Task<bool> IsEmployeeExist(Guid id)
        => await _dbContext.Employees.AnyAsync(e => e.Id == id);
}