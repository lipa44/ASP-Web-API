using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using ReportsDataAccessLayer.DataBase;
using ReportsDataAccessLayer.Services.Interfaces;
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

    public async Task<WorkTeam> RegisterWorkTeam(WorkTeam workTeam)
    {
        ArgumentNullException.ThrowIfNull(workTeam);

        if (IsWorkTeamExist(workTeam.Id).Result)
            throw new Exception($"Work team {workTeam} to register is already exist");

        EntityEntry<WorkTeam> newWorkTeam = await _dbContext.WorkTeams.AddAsync(workTeam);
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

    private async Task<bool> IsWorkTeamExist(Guid id)
        => await _dbContext.WorkTeams.AnyAsync(wt => wt.Id == id);
}