using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;
using ReportsDataAccessLayer.DataBase;
using ReportsDataAccessLayer.Services.Interfaces;
using ReportsLibrary.Employees;
using ReportsLibrary.Entities;
using ReportsLibrary.Tasks;
using ReportsLibrary.Tools;

namespace ReportsDataAccessLayer.Services;

public class SprintsService : ISprintsService
{
    private readonly ReportsDbContext _dbContext;

    public SprintsService(ReportsDbContext context) => _dbContext = context;

    public async Task<List<Sprint>> GetSprintsAsync()
        => await _dbContext.Sprints.ToListAsync();

    public async Task<Sprint> FindSprintByIdAsync(Guid sprintId)
        => await _dbContext.Sprints.SingleOrDefaultAsync(sprint => sprint.SprintId == sprintId);

    public async Task<Sprint> GetSprintByIdAsync(Guid sprintId)
        => await _dbContext.Sprints
               .SingleOrDefaultAsync(sprint => sprint.SprintId == sprintId)
           ?? throw new ReportsException($"Sprint with id {sprintId} doesn't exist");

    public async Task<Sprint> CreateSprint(DateTime expirationDate, Guid workTeamId, Guid changerId)
    {
        await using IDbContextTransaction transaction = await _dbContext.Database.BeginTransactionAsync();

        try
        {
            await transaction.CreateSavepointAsync("BeforeSprintCreated");

            WorkTeam workTeamToAddSprint = await GetWorkTeamByIdAsync(workTeamId);
            Employee changerToSetSprint = await GetEmployeeByIdAsync(changerId);
            Sprint sprintToAddInTeam = new (expirationDate);

            workTeamToAddSprint.AddSprint(changerToSetSprint, sprintToAddInTeam);

            EntityEntry<Sprint> newSprint = await _dbContext.Sprints.AddAsync(sprintToAddInTeam);
            _dbContext.WorkTeams.Update(workTeamToAddSprint);

            await _dbContext.SaveChangesAsync();
            await transaction.CommitAsync();

            return newSprint.Entity;
        }
        catch (Exception)
        {
            await transaction.RollbackToSavepointAsync("BeforeSprintCreated");
            throw;
        }
    }

    public async Task<Sprint> AddSprintToWorkTeam(Guid changerId, Guid sprintId, Guid workTeamId)
    {
        await using IDbContextTransaction transaction = await _dbContext.Database.BeginTransactionAsync();

        try
        {
            await transaction.CreateSavepointAsync("BeforeWorkTeamAdded");

            Sprint sprintToAddInWorkTeam = await GetSprintByIdAsync(sprintId);
            WorkTeam workTeamToAddSprintIn = await GetWorkTeamByIdAsync(workTeamId);
            Employee employeeToAddSprint = await GetEmployeeByIdAsync(changerId);

            workTeamToAddSprintIn.AddSprint(employeeToAddSprint, sprintToAddInWorkTeam);

            EntityEntry<Sprint> updatedSprint = _dbContext.Sprints.Update(sprintToAddInWorkTeam);

            await _dbContext.SaveChangesAsync();
            await transaction.CommitAsync();

            return updatedSprint.Entity;
        }
        catch (Exception)
        {
            await transaction.RollbackToSavepointAsync("BeforeWorkTeamAdded");
            throw;
        }
    }

    public async Task<Sprint> AddTaskToSprint(Guid sprintId, Guid taskId)
    {
        await using IDbContextTransaction transaction = await _dbContext.Database.BeginTransactionAsync();

        try
        {
            await transaction.CreateSavepointAsync("BeforeTaskAdded");

            Sprint sprintToAddTaskIn = await GetSprintByIdAsync(sprintId);
            ReportsTask taskToAddIntoSprint = await GetTaskByIdAsync(taskId);

            sprintToAddTaskIn.AddTask(taskToAddIntoSprint);

            EntityEntry<Sprint> updatedSprint = _dbContext.Sprints.Update(sprintToAddTaskIn);

            await _dbContext.SaveChangesAsync();
            await transaction.CommitAsync();

            return updatedSprint.Entity;
        }
        catch (Exception)
        {
            await transaction.RollbackToSavepointAsync("BeforeTaskAdded");
            throw;
        }
    }

    public async Task<Sprint> RemoveTaskFromSprint(Guid sprintId, Guid taskId)
    {
        await using IDbContextTransaction transaction = await _dbContext.Database.BeginTransactionAsync();

        try
        {
            await transaction.CreateSavepointAsync("BeforeTaskRemoved");

            Sprint sprintToAddTaskIn = await GetSprintByIdAsync(sprintId);
            ReportsTask taskToAddIntoSprint = await GetTaskByIdAsync(taskId);

            sprintToAddTaskIn.RemoveTask(taskToAddIntoSprint);

            EntityEntry<Sprint> updatedSprint = _dbContext.Sprints.Update(sprintToAddTaskIn);

            await _dbContext.SaveChangesAsync();
            await transaction.CommitAsync();

            return updatedSprint.Entity;
        }
        catch (Exception)
        {
            await transaction.RollbackToSavepointAsync("BeforeTaskRemoved");
            throw;
        }
    }

    private async Task<WorkTeam> GetWorkTeamByIdAsync(Guid workTeamId)
        => await _dbContext.WorkTeams.SingleOrDefaultAsync(workTeam => workTeam.Id == workTeamId)
           ?? throw new ReportsException($"WorkTeam with Id {workTeamId} doesn't exist");

    private async Task<ReportsTask> GetTaskByIdAsync(Guid taskId)
        => await _dbContext.Tasks.SingleOrDefaultAsync(task => task.Id == taskId)
           ?? throw new ReportsException($"Task with Id {taskId} doesn't exist");

    private async Task<Employee> GetEmployeeByIdAsync(Guid employeeId)
        => await _dbContext.Employees.SingleOrDefaultAsync(employee => employee.Id == employeeId)
           ?? throw new ReportsException($"Employee with Id {employeeId} doesn't exist");

    private async Task<bool> IsSprintExistAsync(Guid sprintId)
        => await _dbContext.Sprints.AnyAsync(sprint => sprint.SprintId == sprintId);
}