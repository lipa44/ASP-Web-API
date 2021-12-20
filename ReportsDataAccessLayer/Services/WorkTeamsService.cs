using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using ReportsDataAccessLayer.DataBase;
using ReportsDataAccessLayer.Services.Interfaces;
using ReportsLibrary.Employees;
using ReportsLibrary.Entities;
using ReportsLibrary.Tools;

namespace ReportsDataAccessLayer.Services;

public class WorkTeamsService : IWorkTeamsService
{
    private readonly ReportsDbContext _dbContext;

    public WorkTeamsService(ReportsDbContext context) => _dbContext = context;

    public async Task<List<WorkTeam>> GetWorkTeams()
        => await _dbContext.WorkTeams.ToListAsync();

    public async Task<WorkTeam> GetWorkTeamById(Guid workTeamId)
        => await _dbContext.WorkTeams
            .Include(workTeam => workTeam.Sprints)
            .Include(workTeam => workTeam.EmployeesAboba)
            .Include(workTeam => workTeam.Report)
            .SingleOrDefaultAsync(workTeam => workTeam.Id == workTeamId)
           ?? throw new ReportsException($"WorkTeam with Id {workTeamId} doesn't exist");

    public async Task<WorkTeam> RegisterWorkTeam(Guid leadId, string workTeamName)
    {
        Employee teamLead = await GetEmployeeByIdAsync(leadId);

        var newTeam = new WorkTeam(teamLead, workTeamName);
        teamLead.SetWorkTeam(newTeam);

        EntityEntry<WorkTeam> newWorkTeam = await _dbContext.WorkTeams.AddAsync(newTeam);
        _dbContext.Employees.Update(teamLead);

        await _dbContext.SaveChangesAsync();

        return newWorkTeam.Entity;
    }

    public async Task<WorkTeam> AddEmployeeToTeam(Guid employeeId, Guid changerId, Guid teamId)
    {
        Employee employeeToAddIntoTeam = await GetEmployeeByIdAsync(employeeId);
        Employee changerToAddIntoTeam = await GetEmployeeByIdAsync(employeeId);
        WorkTeam teamToAddIn = await GetWorkTeamById(teamId);

        employeeToAddIntoTeam.SetWorkTeam(teamToAddIn);
        teamToAddIn.AddEmployee(employeeToAddIntoTeam, changerToAddIntoTeam);
        _dbContext.Update(employeeToAddIntoTeam);

        await _dbContext.SaveChangesAsync();

        return teamToAddIn;
    }

    public async Task<WorkTeam> RemoveEmployeeFromTeam(Guid employeeId, Guid changerId, Guid teamId)
    {
        Employee employeeToRemoveFromTeam = await GetEmployeeByIdAsync(employeeId);
        Employee changerToRemoveFromTeam = await GetEmployeeByIdAsync(employeeId);
        WorkTeam teamToRemoveFrom = await GetWorkTeamById(teamId);

        employeeToRemoveFromTeam.RemoveWorkTeam(teamToRemoveFrom);
        teamToRemoveFrom.RemoveEmployee(employeeToRemoveFromTeam, changerToRemoveFromTeam);
        _dbContext.Update(employeeToRemoveFromTeam);

        await _dbContext.SaveChangesAsync();

        return teamToRemoveFrom;
    }

    public async Task AddSprintToTeam(Guid workTeamId, Guid changerId, DateTime sprintExpirationDate)
    {
        WorkTeam workTeamToAddSprint = await GetWorkTeamById(workTeamId);
        Employee employeeToAddSprint = await GetEmployeeByIdAsync(changerId);

        Sprint newSprint = new (sprintExpirationDate);
        workTeamToAddSprint.AddSprint(employeeToAddSprint, newSprint);

        _dbContext.Sprints.Add(newSprint);
        _dbContext.Update(workTeamToAddSprint);

        await _dbContext.SaveChangesAsync();
    }

    public async Task RemoveSprintFromTeam(Guid workTeamId, Guid changerId, Guid sprintId)
    {
        WorkTeam workTeamToRemove = await GetWorkTeamById(workTeamId);
        Employee employeeToAddSprint = await GetEmployeeByIdAsync(changerId);
        Sprint sprintToRemove = await GetSprintByIdAsync(sprintId);

        workTeamToRemove.RemoveSprint(employeeToAddSprint, sprintToRemove);

        _dbContext.Sprints.Remove(sprintToRemove);
        _dbContext.Update(workTeamToRemove);

        await _dbContext.SaveChangesAsync();
    }

    public async Task<WorkTeam> RemoveWorkTeam(Guid workTeamId)
    {
        WorkTeam workTeamToRemove = await GetWorkTeamById(workTeamId);

        _dbContext.WorkTeams.Remove(workTeamToRemove);

        await _dbContext.SaveChangesAsync();

        return workTeamToRemove;
    }

    public async Task<WorkTeam> GenerateReport(Guid workTeamId, Guid changerId)
    {
        WorkTeam workTeamToGenerateReport = await GetWorkTeamById(workTeamId);
        Employee employeeToGenerateReport = await GetEmployeeByIdAsync(changerId);

        Report updatedReport = workTeamToGenerateReport.GenerateReport(employeeToGenerateReport);

        _dbContext.WorkTeams.Update(workTeamToGenerateReport);
        _dbContext.Reports.Update(updatedReport);
        _dbContext.Employees.Update(employeeToGenerateReport);

        return workTeamToGenerateReport;
    }

    private async Task<bool> HasTeamLeadWorkTeam(Guid teamLeadId)
        => await _dbContext.WorkTeams.AnyAsync(workTeam => workTeam.TeamLeadId == teamLeadId);

    private async Task<Employee> GetEmployeeByIdAsync(Guid employeeId)
        => await _dbContext.Employees.SingleOrDefaultAsync(employee => employee.Id == employeeId)
           ?? throw new ReportsException($"Employee with Id {employeeId} doesn't exist");

    private async Task<Sprint> GetSprintByIdAsync(Guid sprintId)
        => await _dbContext.Sprints.SingleOrDefaultAsync(sprint => sprint.SprintId == sprintId)
           ?? throw new ReportsException($"Sprint with Id {sprintId} doesn't exist");
}