using DataAccess.DataBase;
using Domain.Entities;
using Domain.Tools;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;

namespace DataAccess.Repositories.WorkTeams;

public class WorkTeamsRepository : IWorkTeamsRepository
{
    private readonly ReportsDbContext _dbContext;

    public WorkTeamsRepository(ReportsDbContext context) => _dbContext = context;

    public async Task<IReadOnlyCollection<WorkTeam>> GetAll()
        => await _dbContext.WorkTeams
            .Include(workTeam => workTeam.TeamLead)
            .ToListAsync();

    public async Task<WorkTeam> FindItem(Guid id)
        => await _dbContext.WorkTeams
            .Include(workTeam => workTeam.Sprints)
            .Include(workTeam => workTeam.Employees)
            .Include(workTeam => workTeam.TeamLead)
            .SingleOrDefaultAsync(workTeam => workTeam.Id == id);

    public async Task<WorkTeam> GetItem(Guid id)
        => await FindItem(id)
           ?? throw new ReportsException($"WorkTeam with Id {id} doesn't exist");

    public async Task<WorkTeam> Create(WorkTeam item)
    {
        await using IDbContextTransaction transaction = await _dbContext.Database.BeginTransactionAsync();

        try
        {
            await transaction.CreateSavepointAsync("BeforeTeamRegistered");

            // Employee teamLead = await GetEmployeeByIdAsync(item.TeamLead.Id);
            // teamLead.SetWorkTeam(newTeam);
            EntityEntry<WorkTeam> newWorkTeam = _dbContext.WorkTeams.Add(item);

            // _dbContext.Employees.Update(teamLead);
            await _dbContext.SaveChangesAsync();
            await transaction.CommitAsync();

            return newWorkTeam.Entity;
        }
        catch (Exception)
        {
            await transaction.RollbackToSavepointAsync("BeforeTeamRegistered");
            throw;
        }
    }

    public Task<WorkTeam> Update(WorkTeam item)
    {
        throw new NotImplementedException();
    }

    public async Task<WorkTeam> Delete(Guid id)
    {
        await using IDbContextTransaction transaction = await _dbContext.Database.BeginTransactionAsync();

        try
        {
            await transaction.CreateSavepointAsync("BeforeWorkTeamRemoved");

            WorkTeam workTeamToRemove = await GetItem(id);

            _dbContext.WorkTeams.Remove(workTeamToRemove);

            await _dbContext.SaveChangesAsync();
            await transaction.CommitAsync();

            return workTeamToRemove;
        }
        catch (Exception)
        {
            await transaction.RollbackToSavepointAsync("BeforeWorkTeamRemoved");
            throw;
        }
    }

    public void Save(WorkTeam item)
    {
        throw new NotImplementedException();
    }

    public async Task<Employee> GetEmployeeByIdAsync(Guid employeeId)
        => await _dbContext.Employees
               .SingleOrDefaultAsync(employee => employee.Id == employeeId)
           ?? throw new ReportsException($"Employee with Id {employeeId} doesn't exist");

    public void Dispose()
    {
        _dbContext?.Dispose();
    }
}