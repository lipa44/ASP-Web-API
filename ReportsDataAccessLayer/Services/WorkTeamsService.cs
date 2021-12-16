using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using ReportsDataAccessLayer.DataBase;
using ReportsDataAccessLayer.Services.Interfaces;
using ReportsLibrary.Employees;
using ReportsLibrary.Entities;

namespace ReportsDataAccessLayer.Services;

public class WorkTeamsService : IWorkTeamService
{
    private readonly ReportsDbContext _dbContext;

    public WorkTeamsService(ReportsDbContext context)
    {
        _dbContext = context;
    }

    public ActionResult<List<WorkTeam>> GetWorkTeams() => _dbContext.WorkTeams.ToList();
    public async Task<WorkTeam> GetWorkTeamById(Guid workTeamId)
    {
        if (await IsWorkTeamExist(workTeamId))
            throw new Exception($"Work team with Id {workTeamId} doesn't exist");

        return await _dbContext.WorkTeams.SingleAsync(team => team.Id == workTeamId);
    }

    public async Task<WorkTeam> RegisterWorkTeam(Guid leadId, string workTeamName)
    {
        Employee teamLead = await GetEmployeeByIdAsync(leadId);

        EntityEntry<WorkTeam> newWorkTeam
            = await _dbContext.WorkTeams.AddAsync(new WorkTeam(teamLead, workTeamName));

        await _dbContext.SaveChangesAsync();

        return newWorkTeam.Entity;
    }

    public async void RemoveWorkTeam(WorkTeam workTeam)
    {
        ArgumentNullException.ThrowIfNull(workTeam);

        if (!IsWorkTeamExist(workTeam.Id).Result)
            throw new Exception($"Work team {workTeam} to remove doesn't exist");

        _dbContext.WorkTeams.Remove(workTeam);
        await _dbContext.SaveChangesAsync();
    }

    private async Task<bool> IsWorkTeamExist(Guid workTeamId)
        => await _dbContext.WorkTeams.AnyAsync(wt => wt.Id == workTeamId);

    private async Task<Employee> GetEmployeeByIdAsync(Guid employeeId)
        => await _dbContext.Employees.SingleAsync(lead => lead.Id == employeeId);
}