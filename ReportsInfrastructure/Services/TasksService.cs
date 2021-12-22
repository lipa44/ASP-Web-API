using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;
using ReportsDataAccess.DataBase;
using ReportsDomain.Employees;
using ReportsDomain.Tasks;
using ReportsDomain.Tasks.TaskChangeCommands;
using ReportsDomain.Tools;
using ReportsInfrastructure.Services.Interfaces;

namespace ReportsInfrastructure.Services;

public class TasksService : ITasksService
{
    private readonly ReportsDbContext _dbContext;

    public TasksService(ReportsDbContext context) => _dbContext = context;

    public Task<List<ReportsTask>> GetTasks() => _dbContext.Tasks
        .Include(task => task.Owner)
        .ToListAsync();

    public async Task<ReportsTask> FindTaskById(Guid taskId) =>
        await _dbContext.Tasks.SingleOrDefaultAsync(task => task.Id == taskId);

    public async Task<ReportsTask> GetTaskById(Guid taskId)
        => await _dbContext.Tasks
            .Include(task => task.Owner)
            .SingleOrDefaultAsync(task => task.Id == taskId)
           ?? throw new ReportsException($"Task with id {taskId} doesn't exist");

    public async Task<ReportsTask> CreateTask(string taskName)
    {
        await using IDbContextTransaction transaction = await _dbContext.Database.BeginTransactionAsync();

        try
        {
            await transaction.CreateSavepointAsync("BeforeSetAsDone");

            var newTask = new ReportsTask(taskName);

            EntityEntry<ReportsTask> newTaskEntry = await _dbContext.Tasks.AddAsync(newTask);

            await _dbContext.SaveChangesAsync();
            await transaction.CommitAsync();

            return newTaskEntry.Entity;
        }
        catch (Exception)
        {
            await transaction.RollbackToSavepointAsync("BeforeSetAsDone");
            throw;
        }
    }

    public async Task<ReportsTask> RemoveTaskById(Guid taskId)
    {
        await using IDbContextTransaction transaction = await _dbContext.Database.BeginTransactionAsync();

        try
        {
            await transaction.CreateSavepointAsync("BeforeTaskRemoved");

            ReportsTask taskToRemove = await GetTaskById(taskId);

            _dbContext.Tasks.Remove(taskToRemove);

            await _dbContext.SaveChangesAsync();
            await transaction.CommitAsync();

            return taskToRemove;
        }
        catch (Exception)
        {
            await transaction.RollbackToSavepointAsync("BeforeTaskRemoved");
            throw;
        }
    }

    public async Task<ReportsTask> UseChangeTaskCommand(Guid taskId, Guid changerId, ITaskCommand command)
    {
        await using IDbContextTransaction transaction = await _dbContext.Database.BeginTransactionAsync();

        try
        {
            await transaction.CreateSavepointAsync("BeforeTaskModified");

            ReportsTask taskToUpdate = await GetTaskById(taskId);
            Employee changerToUpdateTask = await GetEmployeeFromDbAsync(changerId);

            if (command is SetTaskOwnerCommand setOwnerCommand)
            {
                Guid newTaskOwnerId = setOwnerCommand.NewImplementorId;
                Employee newTaskOwner = await GetEmployeeFromDbAsync(newTaskOwnerId);
                setOwnerCommand.NewImplementor = newTaskOwner;
                command.Execute(changerToUpdateTask, taskToUpdate);
                newTaskOwner.AddTask(taskToUpdate);

                _dbContext.Update(newTaskOwner);
            }
            else
            {
                command.Execute(changerToUpdateTask, taskToUpdate);
            }

            _dbContext.Update(taskToUpdate);

            await _dbContext.SaveChangesAsync();
            await transaction.CommitAsync();

            return taskToUpdate;
        }
        catch (Exception)
        {
            await transaction.RollbackToSavepointAsync("BeforeTaskModified");
            throw;
        }
    }

    public async Task<IReadOnlyCollection<ReportsTask>> FindTasksByCreationTime(DateTime creationTime)
        => (await GetTasks())
                .Where(t => t.CreationTime.Date == creationTime.Date).ToList();

    public async Task<IReadOnlyCollection<ReportsTask>> FindTasksByModificationDate(DateTime modificationTime)
        => (await GetTasks())
                .Where(t => t.ModificationTime.Date == modificationTime.Date).ToList();

    public async Task<IReadOnlyCollection<ReportsTask>> FindTasksByEmployeeId(Guid employeeId)
        => (await GetTasks())
                .Where(task => employeeId == task.OwnerId).ToList();

    public async Task<IReadOnlyCollection<ReportsTask>> FindsTaskModifiedByEmployeeId(Guid employeeId)
        => (await GetTasks())
                .Where(t => t.Modifications
                    .Any(m => m.ChangerId == employeeId)).ToList();

    public async Task<IReadOnlyCollection<ReportsTask>> FindTasksCreatedByEmployeeSubordinates(Guid employeeId)
        => (await GetTasks())
                .Where(t => t.Owner?.ChiefId == employeeId).ToList();

    private async Task<Employee> GetEmployeeFromDbAsync(Guid employeeId) =>
        await _dbContext.Employees.SingleOrDefaultAsync(employee => employee.Id == employeeId)
            ?? throw new ReportsException($"Employee with Id {employeeId} doesn't exist");
}