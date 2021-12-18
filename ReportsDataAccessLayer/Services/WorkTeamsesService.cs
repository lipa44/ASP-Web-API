using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using ReportsDataAccessLayer.DataBase;
using ReportsDataAccessLayer.Services.Interfaces;
using ReportsLibrary.Employees;
using ReportsLibrary.Entities;

namespace ReportsDataAccessLayer.Services;

public class WorkTeamsesService : IWorkTeamsService
{
    private readonly ReportsDbContext _dbContext;

    public WorkTeamsesService(ReportsDbContext context) => _dbContext = context;

    public async Task<List<WorkTeam>> GetWorkTeams()
        => await _dbContext.WorkTeams.ToListAsync();

    public async Task<WorkTeam> GetWorkTeamById(Guid workTeamId)
        => await _dbContext.WorkTeams
            .Include(t => t.Sprints)
            .Include(t => t.EmployeesAboba).SingleAsync(team => team.Id == workTeamId);

    public async Task<WorkTeam> RegisterWorkTeam(Guid leadId, string workTeamName)
    {
        Employee teamLead = await GetEmployeeByIdAsync(leadId);

        if (IsWorkTeamExist(leadId).Result)
            throw new Exception($"{teamLead} already has his own work team");

        EntityEntry<WorkTeam> newWorkTeam
            = await _dbContext.WorkTeams.AddAsync(new WorkTeam(teamLead, workTeamName));

        await _dbContext.SaveChangesAsync();

        return newWorkTeam.Entity;
    }

    public async void AddEmployeeToTeam(Guid employeeId, Guid changerId, Guid teamId)
    {
        Employee employeeToAdd = await GetEmployeeByIdAsync(employeeId);
        WorkTeam teamToAddIn = await GetWorkTeamById(teamId);

        if (teamToAddIn.TeamLeadId != changerId)
            throw new Exception("Only team lead has permission to remove employees.");

        employeeToAdd.SetWorkTeam(teamToAddIn);
        teamToAddIn.AddEmployee(employeeToAdd);
        _dbContext.Update(employeeToAdd);

        await _dbContext.SaveChangesAsync();
    }

    public async void RemoveEmployeeFromTeam(Guid employeeId, Guid changerId, Guid teamId)
    {
        Employee employeeToRemove = await GetEmployeeByIdAsync(employeeId);
        WorkTeam teamToRemoveFrom = await GetWorkTeamById(teamId);

        if (teamToRemoveFrom.TeamLeadId != changerId)
            throw new Exception("Only team lead has permission to remove employees.");

        employeeToRemove.RemoveWorkTeam(teamToRemoveFrom);
        teamToRemoveFrom.RemoveEmployee(employeeToRemove);
        _dbContext.Update(employeeToRemove);

        await _dbContext.SaveChangesAsync();
    }

    public async Task AddSprintToTeam(Guid workTeamId, Guid changerId, DateTime sprintExpirationDate)
    {
        WorkTeam workTeamToAddSprint = await GetWorkTeamById(workTeamId);
        Employee employeeToAddSprint = await GetEmployeeByIdAsync(changerId);

        if (workTeamToAddSprint.TeamLeadId != changerId)
            throw new Exception("Only team lead has permission to add sprints.");

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

        if (workTeamToRemove.TeamLeadId != changerId)
            throw new Exception("Only team lead has permission to add sprints.");

        workTeamToRemove.RemoveSprint(employeeToAddSprint, sprintToRemove);

        _dbContext.Sprints.Remove(sprintToRemove);
        _dbContext.Update(workTeamToRemove);

        await _dbContext.SaveChangesAsync();
    }

    public async void RemoveWorkTeam(WorkTeam workTeam)
    {
        ArgumentNullException.ThrowIfNull(workTeam);

        if (!IsWorkTeamExist(workTeam.Id).Result)
            throw new Exception($"Work team {workTeam} to remove doesn't exist");

        _dbContext.WorkTeams.Remove(workTeam);

        await _dbContext.SaveChangesAsync();
    }

    public async Task<WorkTeam> GenerateReport(Guid workTeamId, Guid changerId)
    {
        WorkTeam workTeamToGenerateReport = await GetWorkTeamById(workTeamId);
        Employee employeeToGenerateReport = await GetEmployeeByIdAsync(changerId);

        return workTeamToGenerateReport;
    }

    private async Task<bool> IsWorkTeamExist(Guid teamLeadId)
        => await _dbContext.WorkTeams.AnyAsync(wt => wt.TeamLeadId == teamLeadId);

    private async Task<Employee> GetEmployeeByIdAsync(Guid employeeId)
        => await _dbContext.Employees.SingleAsync(lead => lead.Id == employeeId);

    private async Task<Sprint> GetSprintByIdAsync(Guid sprintId)
        => await _dbContext.Sprints.SingleAsync(sprint => sprint.SprintId == sprintId);
}