using DataAccess.DataBase;
using Domain.Entities;
using Domain.Tools;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;

namespace DataAccess.Repositories.Employees;

public class EmployeesRepository : IEmployeesRepository
{
    private readonly ReportsDbContext _dbContext;

    public EmployeesRepository(ReportsDbContext context) => _dbContext = context;

    public async Task<IReadOnlyCollection<Employee>> GetAll()
        => await _dbContext.Employees.ToListAsync();

    public async Task<Employee> GetItem(Guid id)
        => await FindItem(id)
           ?? throw new ReportsException($"Employee with id {id} doesn't exist");

    public async Task<Employee> FindItem(Guid id)
        => await _dbContext.Employees
            .Include(employee => employee.Tasks)
            .Include(employee => employee.Report)
            .Include(employee => employee.Subordinates)
            .SingleOrDefaultAsync(employee => employee.Id == id);

    public async Task<Employee> Create(Employee item)
    {
        await using IDbContextTransaction transaction = await _dbContext.Database.BeginTransactionAsync();

        try
        {
            await transaction.CreateSavepointAsync("BeforeRegisteringEmployee");

            if (IsEmployeeExistAsync(item.Id).Result)
                throw new ReportsException($"Employee {item.Id} to add is already exist");

            EntityEntry<Employee> newEmployee = await _dbContext.Employees.AddAsync(item);

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

    public async Task<Employee> Update(Employee item)
    {
        await using IDbContextTransaction transaction = await _dbContext.Database.BeginTransactionAsync();

        try
        {
            await transaction.CreateSavepointAsync("BeforeUpdatingEmployee");

            if (!IsEmployeeExistAsync(item.Id).Result)
                throw new ReportsException($"Employee {item.Id} to update doesn't exist");

            EntityEntry<Employee> updatedEmployee = _dbContext.Employees.Update(item);

            await _dbContext.SaveChangesAsync();
            await transaction.CommitAsync();

            return updatedEmployee.Entity;
        }
        catch (Exception)
        {
            await transaction.RollbackToSavepointAsync("BeforeUpdatingEmployee");
            throw;
        }
    }

    public async Task<Employee> Delete(Guid id)
    {
        await using IDbContextTransaction transaction = await _dbContext.Database.BeginTransactionAsync();

        try
        {
            await transaction.CreateSavepointAsync("BeforeEmployeeRemoved");

            Employee employeeToRemove = await GetItem(id);

            _dbContext.Employees.Remove(employeeToRemove);

            await _dbContext.SaveChangesAsync();
            await transaction.CommitAsync();

            return employeeToRemove;
        }
        catch (Exception)
        {
            await transaction.RollbackToSavepointAsync("BeforeEmployeeRemoved");
            throw;
        }
    }

    public async Task<Report> CreateEmptyReport(Guid employeeId)
    {
        await using IDbContextTransaction transaction = await _dbContext.Database.BeginTransactionAsync();

        try
        {
            await transaction.CreateSavepointAsync("BeforeReportCreated");

            Employee employee = await GetItem(employeeId);
            Report report = employee.CreateReport();

            _dbContext.Reports.Add(report);

            return report;
        }
        catch (Exception)
        {
            await transaction.RollbackToSavepointAsync("BeforeReportCreated");
            throw;
        }
    }

    public void Save(Employee item)
    {
        throw new NotImplementedException();
    }

    public async Task<IReadOnlyCollection<Employee>> GetEmployeeSubordinatesById(Guid employeeId)
        => await _dbContext.Employees
            .Where(e => e.ChiefId == employeeId).ToListAsync();

    private async Task<bool> IsEmployeeExistAsync(Guid employeeId)
        => await _dbContext.Employees.AnyAsync(employee => employee.Id == employeeId);
}