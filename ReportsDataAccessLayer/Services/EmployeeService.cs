using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using ReportsDataAccessLayer.DataBase;
using ReportsDataAccessLayer.Services.Interfaces;
using ReportsLibrary.Employees;
using ReportsLibrary.Tasks;

namespace ReportsDataAccessLayer.Services;

public class EmployeeService : IEmployeeService
{
    private readonly ReportsDbContext _dbContext;

    public EmployeeService(ReportsDbContext context) => _dbContext = context;

    public async Task<List<Employee>> GetEmployees() => await _dbContext.Employees
        .ToListAsync();

    public async Task<Employee> RegisterEmployee(Employee employee)
    {
        ArgumentNullException.ThrowIfNull(employee);

        if (IsEmployeeExist(employee.Id).Result)
            throw new Exception($"Employee {employee} to add is already exist");

        EntityEntry<Employee> newEmployee = await _dbContext.Employees.AddAsync(employee);

        await _dbContext.SaveChangesAsync();

        return newEmployee.Entity;
    }

    public async Task<Employee> SetChief(Employee employee, Employee chief)
    {
        ArgumentNullException.ThrowIfNull(employee);
        ArgumentNullException.ThrowIfNull(chief);

        employee.SetChief(chief);
        _dbContext.Update(employee);

        await _dbContext.SaveChangesAsync();

        return employee;
    }

    public async void RemoveEmployee(Employee employee)
    {
        ArgumentNullException.ThrowIfNull(employee);

        if (!await IsEmployeeExist(employee.Id))
            throw new Exception($"Employee {employee} to remove doesn't exist");

        // TODO: Check is need to be deleted
        foreach (ReportsTask task in _dbContext.Tasks.Where(t => t.ReportsTaskId == employee.Id))
            _dbContext.Tasks.Remove(task);

        _dbContext.Remove(employee);

        await _dbContext.SaveChangesAsync();
    }

    public async Task<Employee> GetEmployeeById(Guid id)
    {
        if (!IsEmployeeExist(id).Result)
            throw new Exception("Employee to get doesn't exist");

        return await _dbContext.Employees.SingleAsync(e => e.Id == id);
    }

    public async Task<Employee> FindEmployeeById(Guid id) =>
        await _dbContext.Employees.SingleOrDefaultAsync(e => e.Id == id);

    private async Task<bool> IsEmployeeExist(Guid id)
        => await _dbContext.Employees.AnyAsync(e => e.Id == id);
}