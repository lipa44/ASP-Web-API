using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using ReportsDataAccessLayer.DataBase;
using ReportsDataAccessLayer.Services.Interfaces;
using ReportsLibrary.Employees;
using ReportsLibrary.Entities;
using ReportsLibrary.Enums;
using ReportsLibrary.Tasks;
using ReportsLibrary.Tools;

namespace ReportsDataAccessLayer.Services;

public class EmployeeService : IEmployeeService
{
    private readonly ReportsDbContext _dbContext;

    public EmployeeService(ReportsDbContext context) => _dbContext = context;

    public async Task<List<Employee>> GetEmployees()
        => await _dbContext.Employees.ToListAsync();

    public async Task<Employee> FindEmployeeByIdAsync(Guid id) =>
        await _dbContext.Employees.SingleOrDefaultAsync(e => e.Id == id);

    public async Task<Employee> GetEmployeeByIdAsync(Guid id)
    {
        if (!IsEmployeeExistAsync(id).Result)
            throw new Exception("Employee to get doesn't exist");

        return await _dbContext.Employees
            .Include(e => e.Tasks)
            .Include(e => e.Report)
            .Include(e => e.Subordinates)
            .SingleAsync(e => e.Id == id);
    }

    public async Task<Employee> RegisterEmployee(Guid employeeToRegisterId, string name, string surname, EmployeeRoles role)
    {
        if (IsEmployeeExistAsync(employeeToRegisterId).Result)
            throw new Exception($"Employee {employeeToRegisterId} to add is already exist");

        EntityEntry<Employee> newEmployee =
            await _dbContext.Employees.AddAsync(new Employee(name, surname, employeeToRegisterId, role));

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
        WorkTeam teamToAddEmployee = await GetWorkTeamByIdAsync(workTeamId);

        if (teamToAddEmployee.TeamLeadId != changerId)
            throw new PermissionDeniedException("Only team lead can add employees to the work team");

        teamToAddEmployee.AddEmployee(employeeToSetTeam);
        employeeToSetTeam.SetWorkTeam(teamToAddEmployee);

        _dbContext.Update(employeeToSetTeam);
        _dbContext.Update(teamToAddEmployee);

        await _dbContext.SaveChangesAsync();

        return employeeToSetTeam;
    }

    public async Task<Employee> RemoveWorkTeam(Guid employeeId, Guid changerId, Guid workTeamId)
    {
        Employee employeeToRemoveTeam = await GetEmployeeByIdAsync(employeeId);
        WorkTeam teamToRemoveEmployee = await GetWorkTeamByIdAsync(workTeamId);

        if (teamToRemoveEmployee.TeamLeadId != changerId)
            throw new PermissionDeniedException("Only team lead can remove employees from the work team");

        teamToRemoveEmployee.RemoveEmployee(employeeToRemoveTeam);
        employeeToRemoveTeam.RemoveWorkTeam(teamToRemoveEmployee);

        _dbContext.Update(employeeToRemoveTeam);
        _dbContext.Update(teamToRemoveEmployee);

        await _dbContext.SaveChangesAsync();

        return employeeToRemoveTeam;
    }

    public async void RemoveEmployee(Guid employeeId)
    {
        if (!IsEmployeeExistAsync(employeeId).Result)
            throw new Exception($"Employee {employeeId} to remove doesn't exist");

        Employee employeeToRemove = await GetEmployeeByIdAsync(employeeId);

        _dbContext.Remove(employeeToRemove);

        await _dbContext.SaveChangesAsync();
    }

    public async Task<Report> CommitChangesToReport(Guid employeeId)
    {
        Employee employee = await GetEmployeeByIdAsync(employeeId);
        Report updatedReport = employee.CommitChangesToReport();

        _dbContext.Reports.Update(updatedReport);
        _dbContext.Employees.Update(employee);

        await _dbContext.SaveChangesAsync();

        return updatedReport;
    }

    public async Task<Report> CreateReport(Guid employeeId)
    {
        Employee employee = await GetEmployeeByIdAsync(employeeId);

        Report newReport = employee.CreateReport();

        _dbContext.Reports.Add(newReport);
        _dbContext.Employees.Update(employee);

        await _dbContext.SaveChangesAsync();

        return newReport;
    }

    private async Task<WorkTeam> GetWorkTeamByIdAsync(Guid workTeamId)
        => await _dbContext.WorkTeams.SingleAsync(e => e.Id == workTeamId);

    private async Task<List<ReportsTask>> GetTasksByEmployeeId(Guid employeeId)
        => await _dbContext.Tasks.Where(t => t.OwnerId == employeeId).ToListAsync();

    private async Task<bool> IsEmployeeExistAsync(Guid employeeId)
        => await _dbContext.Employees.AnyAsync(e => e.Id == employeeId);
}