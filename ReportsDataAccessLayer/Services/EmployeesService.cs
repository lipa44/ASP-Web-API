using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using ReportsDataAccessLayer.DataBase;
using ReportsDataAccessLayer.Services.Interfaces;
using ReportsLibrary.Employees;
using ReportsLibrary.Entities;
using ReportsLibrary.Enums;
using ReportsLibrary.Tools;

namespace ReportsDataAccessLayer.Services;

public class EmployeesService : IEmployeesService
{
    private readonly ReportsDbContext _dbContext;

    public EmployeesService(ReportsDbContext context) => _dbContext = context;

    public async Task<List<Employee>> GetEmployeesAsync()
        => await _dbContext.Employees.ToListAsync();

    public async Task<Employee> FindEmployeeByIdAsync(Guid employeeId) =>
        await _dbContext.Employees.SingleOrDefaultAsync(employee => employee.Id == employeeId);

    public async Task<Employee> GetEmployeeByIdAsync(Guid employeeId)
        => await _dbContext.Employees
               .Include(employee => employee.Tasks)
               .Include(employee => employee.Report)
               .Include(employee => employee.Subordinates)
               .SingleOrDefaultAsync(employee => employee.Id == employeeId)
           ?? throw new ReportsException($"Employee with id {employeeId} doesn't exist");

    public async Task<Employee> RegisterEmployee(Guid employeeId, string name, string surname, EmployeeRoles role)
    {
        if (IsEmployeeExistAsync(employeeId).Result)
            throw new ReportsException($"Employee {employeeId} to add is already exist");

        EntityEntry<Employee> newEmployee =
            await _dbContext.Employees.AddAsync(new Employee(name, surname, employeeId, role));

        await _dbContext.SaveChangesAsync();

        return newEmployee.Entity;
    }

    public async Task<Employee> SetChief(Guid employeeId, Guid chiefId)
    {
        Employee employee = await GetEmployeeByIdAsync(employeeId);
        Employee chief = await GetEmployeeByIdAsync(chiefId);

        employee.SetChief(chief);
        chief.AddSubordinate(employee);

        _dbContext.Update(employee);
        _dbContext.Update(chief);

        await _dbContext.SaveChangesAsync();

        return employee;
    }

    public async Task<Employee> SetWorkTeam(Guid employeeId, Guid changerId, Guid workTeamId)
    {
        Employee employeeToSetTeam = await GetEmployeeByIdAsync(employeeId);
        Employee changerToSetTeam = await GetEmployeeByIdAsync(changerId);
        WorkTeam teamToAddEmployee = await GetWorkTeamByIdAsync(workTeamId);

        teamToAddEmployee.AddEmployee(employeeToSetTeam, changerToSetTeam);
        employeeToSetTeam.SetWorkTeam(teamToAddEmployee);

        _dbContext.Update(employeeToSetTeam);
        _dbContext.Update(teamToAddEmployee);

        await _dbContext.SaveChangesAsync();

        return employeeToSetTeam;
    }

    public async Task<Employee> RemoveWorkTeam(Guid employeeId, Guid changerId, Guid workTeamId)
    {
        Employee employeeToRemoveTeam = await GetEmployeeByIdAsync(employeeId);
        Employee changerToRemoveTeam = await GetEmployeeByIdAsync(changerId);
        WorkTeam teamToRemoveEmployee = await GetWorkTeamByIdAsync(workTeamId);

        teamToRemoveEmployee.RemoveEmployee(employeeToRemoveTeam, changerToRemoveTeam);
        employeeToRemoveTeam.RemoveWorkTeam(teamToRemoveEmployee);

        _dbContext.Update(employeeToRemoveTeam);
        _dbContext.Update(teamToRemoveEmployee);

        await _dbContext.SaveChangesAsync();

        return employeeToRemoveTeam;
    }

    public async Task<Employee> RemoveEmployee(Guid employeeId)
    {
        Employee employeeToRemove = await GetEmployeeByIdAsync(employeeId);

        _dbContext.Remove(employeeToRemove);

        await _dbContext.SaveChangesAsync();

        return employeeToRemove;
    }

    private async Task<WorkTeam> GetWorkTeamByIdAsync(Guid workTeamId)
        => await _dbContext.WorkTeams.SingleOrDefaultAsync(workTeam => workTeam.Id == workTeamId)
           ?? throw new ReportsException($"WorkTeam with Id {workTeamId} doesn't exist");

    private async Task<bool> IsEmployeeExistAsync(Guid employeeId)
        => await _dbContext.Employees.AnyAsync(employee => employee.Id == employeeId);
}