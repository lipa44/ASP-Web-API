namespace ReportsInfrastructure.Services;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;
using ReportsDataAccess.DataBase;
using ReportsDomain.Entities;
using ReportsDomain.Enums;
using ReportsDomain.Tools;
using Interfaces;

public class EmployeesService : IEmployeesService
{
    private readonly ReportsDbContext _dbContext;

    public EmployeesService(ReportsDbContext context) => _dbContext = context;

    public async Task<List<Employee>> GetEmployees()
        => await _dbContext.Employees.ToListAsync();

    public async Task<Employee> GetEmployeeById(Guid employeeId)
        => await FindEmployeeById(employeeId)
           ?? throw new ReportsException($"Employee with id {employeeId} doesn't exist");

    public async Task<Employee> FindEmployeeById(Guid employeeId)
        => await _dbContext.Employees
            .Include(employee => employee.Tasks)
            .Include(employee => employee.Report)
            .Include(employee => employee.Subordinates)
            .SingleOrDefaultAsync(employee => employee.Id == employeeId);

    public async Task<List<Employee>> GetEmployeeSubordinatesById(Guid employeeId)
        => await _dbContext.Employees
            .Where(e => e.ChiefId == employeeId).ToListAsync();

    public async Task<Employee> CreateEmployee(Guid employeeId, string name, string surname, EmployeeRoles role)
    {
        await using IDbContextTransaction transaction = await _dbContext.Database.BeginTransactionAsync();

        try
        {
            await transaction.CreateSavepointAsync("BeforeRegisteringEmployee");

            if (IsEmployeeExistAsync(employeeId).Result)
                throw new ReportsException($"Employee {employeeId} to add is already exist");

            EntityEntry<Employee> newEmployee =
                await _dbContext.Employees.AddAsync(new Employee(name, surname, employeeId, role));

            await _dbContext.SaveChangesAsync();
            await transaction.CommitAsync();

            return newEmployee.Entity;
        }
        catch (Exception)
        {
            await transaction.RollbackToSavepointAsync("BeforeRegisteringEmployee");
            throw;
        }
    }

    public async Task<Employee> SetChief(Guid employeeId, Guid chiefId)
    {
        await using IDbContextTransaction transaction = await _dbContext.Database.BeginTransactionAsync();

        try
        {
            await transaction.CreateSavepointAsync("BeforeSetChief");

            if (IsSubordinateExistAsync(chiefId, employeeId).Result)
                throw new ReportsException("Employee to set chief already in subordinate list");

            Employee employee = await GetEmployeeById(employeeId);
            Employee chief = await GetEmployeeById(chiefId);

            employee.AddSubordinate(chief);
            _dbContext.Update(employee);

            await _dbContext.SaveChangesAsync();
            await transaction.CommitAsync();

            return employee;
        }
        catch (Exception)
        {
            await transaction.RollbackToSavepointAsync("BeforeSetChief");
            throw;
        }
    }

    public async Task<Employee> RemoveEmployee(Guid employeeId)
    {
        await using IDbContextTransaction transaction = await _dbContext.Database.BeginTransactionAsync();

        try
        {
            await transaction.CreateSavepointAsync("BeforeEmployeeRemovedFromTeam");

            Employee employeeToRemove = await GetEmployeeById(employeeId);

            _dbContext.Remove(employeeToRemove);

            await _dbContext.SaveChangesAsync();
            await transaction.CommitAsync();

            return employeeToRemove;
        }
        catch (Exception)
        {
            await transaction.RollbackToSavepointAsync("BeforeEmployeeRemovedFromTeam");
            throw;
        }
    }

    private async Task<bool> IsEmployeeExistAsync(Guid employeeId)
        => await _dbContext.Employees.AnyAsync(employee => employee.Id == employeeId);

    private async Task<bool> IsSubordinateExistAsync(Guid chiefId, Guid employeeId)
        => (await GetEmployeeById(employeeId)).ChiefId == chiefId;
}