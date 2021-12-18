using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using ReportsDataAccessLayer.DataBase;
using ReportsDataAccessLayer.Services.Interfaces;
using ReportsLibrary.Employees;
using ReportsLibrary.Tasks;
using ReportsLibrary.Tasks.TaskChangeCommands;

namespace ReportsDataAccessLayer.Services;

public class TasksService : ITasksService
{
    private readonly ReportsDbContext _dbContext;

    public TasksService(ReportsDbContext context) => _dbContext = context;

    public Task<List<ReportsTask>> GetTasks() => _dbContext.Tasks
        .Include(t => t.Owner)
        .ToListAsync();

    public async Task<ReportsTask> FindTaskById(Guid taskId) =>
        await _dbContext.Tasks.SingleOrDefaultAsync(t => t.Id == taskId);

    public async Task<ReportsTask> GetTaskById(Guid taskId)
        => await _dbContext.Tasks
            .Include(t => t.Owner)
            .SingleAsync(t => t.Id == taskId);

    public async Task<ReportsTask> CreateTask(string taskName, Guid creatorId)
    {
        Employee creator = await GetEmployeeFromDbAsync(creatorId);

        var newTask = new ReportsTask(taskName);
        newTask.SetOwner(creator, creator);

        EntityEntry<ReportsTask> newTaskEntry = await _dbContext.Tasks.AddAsync(newTask);

        await _dbContext.SaveChangesAsync();

        return newTaskEntry.Entity;
    }

    public async void RemoveTaskById(Guid taskId)
    {
        if (!IsTaskExist(taskId).Result)
            throw new Exception("Task to remove doesn't exist");

        ReportsTask reportsTaskToRemove = await GetTaskById(taskId);

        _dbContext.Tasks.Remove(reportsTaskToRemove);

        await _dbContext.SaveChangesAsync();
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

    public async void UseChangeTaskCommand(Guid taskId, Guid changerId, ITaskCommand command)
    {
        ReportsTask foundReportsTask = await GetTaskById(taskId);
        Employee foundChanger = await GetEmployeeFromDbAsync(changerId);

        if (command is SetTaskOwnerCommand setOwnerCommand)
        {
            Guid newTaskOwnerId = setOwnerCommand.NewImplementorId;
            Employee newTaskOwner = await GetEmployeeFromDbAsync(newTaskOwnerId);
            setOwnerCommand.NewImplementor = newTaskOwner;
            command.Execute(foundChanger, foundReportsTask);
            newTaskOwner.AddTask(foundReportsTask);

            _dbContext.Update(newTaskOwner);
        }
        else
        {
            command.Execute(foundChanger, foundReportsTask);
        }

        _dbContext.Update(foundReportsTask);

        await _dbContext.SaveChangesAsync();
    }

    private async Task<bool> IsTaskExist(Guid id) => await _dbContext.Tasks.AnyAsync(t => t.Id == id);

    private async Task<Employee> GetEmployeeFromDbAsync(Guid employeeId) =>
        await _dbContext.Employees.SingleAsync(e => e.Id == employeeId);
}