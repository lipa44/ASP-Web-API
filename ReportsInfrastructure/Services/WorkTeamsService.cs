using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;
using ReportsDataAccess.DataBase;
using ReportsDomain.Employees;
using ReportsDomain.Entities;
using ReportsDomain.Tools;
using ReportsInfrastructure.Services.Interfaces;

namespace ReportsInfrastructure.Services;

public class WorkTeamsService : IWorkTeamsService
{
    private readonly ReportsDbContext _dbContext;

    public WorkTeamsService(ReportsDbContext context) => _dbContext = context;

    public async Task<List<WorkTeam>> GetWorkTeams()
        => await _dbContext.WorkTeams.ToListAsync();

    public async Task<WorkTeam> GetWorkTeamById(Guid workTeamId)
        => await _dbContext.WorkTeams
               .Include(workTeam => workTeam.Sprints)
            .Include(workTeam => workTeam.Employees)
            .Include(workTeam => workTeam.TeamLead)
            .SingleOrDefaultAsync(workTeam => workTeam.Id == workTeamId)
           ?? throw new ReportsException($"WorkTeam with Id {workTeamId} doesn't exist");

    public async Task<WorkTeam> RegisterWorkTeam(Guid leadId, string workTeamName)
    {
        await using IDbContextTransaction transaction = await _dbContext.Database.BeginTransactionAsync();

        try
        {
            await transaction.CreateSavepointAsync("BeforeTeamRegistered");

            Employee teamLead = await GetEmployeeByIdAsync(leadId);

            var newTeam = new WorkTeam(teamLead, workTeamName);
            teamLead.SetWorkTeam(newTeam);

            EntityEntry<WorkTeam> newWorkTeam = await _dbContext.WorkTeams.AddAsync(newTeam);
            _dbContext.Employees.Update(teamLead);

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

    public async Task<WorkTeam> AddEmployeeToTeam(Guid employeeId, Guid changerId, Guid teamId)
    {
        await using IDbContextTransaction transaction = await _dbContext.Database.BeginTransactionAsync();

        try
        {
            await transaction.CreateSavepointAsync("BeforeEmployeeAddedToTeam");

            Employee employeeToAddIntoTeam = await GetEmployeeByIdAsync(employeeId);
            Employee changerToAddIntoTeam = await GetEmployeeByIdAsync(changerId);
            WorkTeam teamToAddIn = await GetWorkTeamById(teamId);

            employeeToAddIntoTeam.SetWorkTeam(teamToAddIn);
            teamToAddIn.AddEmployee(employeeToAddIntoTeam, changerToAddIntoTeam);

            _dbContext.Employees.Update(employeeToAddIntoTeam);
            _dbContext.WorkTeams.Update(teamToAddIn);

            await _dbContext.SaveChangesAsync();
            await transaction.CommitAsync();

            return teamToAddIn;
        }
        catch (Exception)
        {
            await transaction.RollbackToSavepointAsync("BeforeEmployeeAddedToTeam");
            throw;
        }
    }

    public async Task<WorkTeam> RemoveEmployeeFromTeam(Guid employeeId, Guid changerId, Guid teamId)
    {
        await using IDbContextTransaction transaction = await _dbContext.Database.BeginTransactionAsync();

        try
        {
            await transaction.CreateSavepointAsync("BeforeEmployeeRemovedFromTeam");

            Employee employeeToRemoveFromTeam = await GetEmployeeByIdAsync(employeeId);
            Employee changerToRemoveFromTeam = await GetEmployeeByIdAsync(changerId);
            WorkTeam teamToRemoveFrom = await GetWorkTeamById(teamId);

            employeeToRemoveFromTeam.RemoveWorkTeam(teamToRemoveFrom);
            teamToRemoveFrom.RemoveEmployee(employeeToRemoveFromTeam, changerToRemoveFromTeam);
            _dbContext.Update(employeeToRemoveFromTeam);

            await _dbContext.SaveChangesAsync();
            await transaction.CommitAsync();

            return teamToRemoveFrom;
        }
        catch (Exception)
        {
            await transaction.RollbackToSavepointAsync("BeforeEmployeeRemovedFromTeam");
            throw;
        }
    }

    public async Task<WorkTeam> RemoveWorkTeam(Guid workTeamId)
    {
        await using IDbContextTransaction transaction = await _dbContext.Database.BeginTransactionAsync();

        try
        {
            await transaction.CreateSavepointAsync("BeforeWorkTeamRemoved");

            WorkTeam workTeamToRemove = await GetWorkTeamById(workTeamId);

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

    private async Task<Employee> GetEmployeeByIdAsync(Guid employeeId)
        => await _dbContext.Employees
               .SingleOrDefaultAsync(employee => employee.Id == employeeId)
           ?? throw new ReportsException($"Employee with Id {employeeId} doesn't exist");
}