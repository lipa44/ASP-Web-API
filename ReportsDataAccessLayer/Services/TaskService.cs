using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using ReportsDataAccessLayer.DataBase;
using ReportsLibrary.Employees;
using ReportsLibrary.Tasks;
using ReportsLibrary.Tasks.TaskChangeCommands;
using ReportsLibrary.Tasks.TaskStates;
using ReportsWebApiLayer.DataBase.Services.Interfaces;

namespace ReportsDataAccessLayer.Services;

public class TaskService : ITaskService
{
    private readonly ReportsDbContext _dbContext;

    public TaskService(ReportsDbContext context) => _dbContext = context;

    public Task<List<ReportsTask>> GetTasks() => _dbContext.Tasks
        .Include(t => t.Modifications)
        .ToListAsync();

    public async Task<ReportsTask> FindTaskById(Guid taskId) =>
        await _dbContext.Tasks.SingleOrDefaultAsync(t => t.TaskId == taskId);

    public async Task<ReportsTask> GetTaskById(Guid taskId)
    {
        if (!await IsTaskExist(taskId))
            throw new Exception($"Task {taskId} to get doesn't exist");

        return await _dbContext.Tasks.SingleAsync(t => t.TaskId == taskId);
    }

    public async void RemoveTaskById(Guid taskId)
    {
        if (!IsTaskExist(taskId).Result)
            throw new Exception("Task to remove doesn't exist");

        ReportsTask reportsTaskToRemove = await _dbContext.Tasks.SingleAsync(t => t.TaskId == taskId);

        _dbContext.Tasks.Remove(reportsTaskToRemove);

        await _dbContext.SaveChangesAsync();
    }

    public async Task<IReadOnlyCollection<ReportsTask>> FindTasksByCreationTime(DateTime creationTime) =>
        await _dbContext.Tasks
            .Where(t => t.CreationTime == creationTime).ToListAsync();

    public async Task<IReadOnlyCollection<ReportsTask>> FindTasksByModificationTime(DateTime modificationTime) =>
        await _dbContext.Tasks
            .Where(t => t.ModificationTime == modificationTime).ToListAsync();

    public async Task<IReadOnlyCollection<ReportsTask>> FindTaskByEmployeeId(Guid employeeId)
    {
        Employee foundEmployee = await GetEmployeeFromDbAsync(employeeId);

        return await _dbContext.Tasks
            .Where(t => foundEmployee.Equals(t.Owner)).ToListAsync();
    }

    public async Task<IReadOnlyCollection<ReportsTask>> FindsTaskModifiedByEmployeeId(Guid employeeId)
    {
        Employee foundEmployee = await GetEmployeeFromDbAsync(employeeId);

        return await _dbContext.Tasks
            .Where(t => t.Modifications
                .Any(m => m.ChangerId.Equals(foundEmployee))).ToListAsync();
    }

    public async Task<IReadOnlyCollection<ReportsTask>> FindTasksCreatedByEmployeeSubordinates(Guid employeeId)
    {
        Employee foundEmployee = await GetEmployeeFromDbAsync(employeeId);

        throw new NotImplementedException();

        // return await _dbContext.Tasks
        //     .Where(t => foundEmployee.Subordinates
        //         .Any(s => s.Equals(t.Owner))).ToListAsync();
    }

    public async Task<ReportsTask> CreateTask(string taskName, Employee owner, Guid ownerId)
    {
        EntityEntry<ReportsTask> newTaskEntry = await _dbContext.Tasks.AddAsync(new (taskName));

        await _dbContext.SaveChangesAsync();

        return newTaskEntry.Entity;
    }

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
        }
        else
        {
            command.Execute(foundChanger, foundReportsTask);
        }

        _dbContext.Update(foundReportsTask);

        await _dbContext.SaveChangesAsync();
    }

    public void SetState(Guid taskId, Guid changerId, TaskState newState)
    {
        throw new NotImplementedException();
    }

    public async void SetContent(Guid taskId, Guid changerId, string newContent)
    {
        ReportsTask foundReportsTask = await GetTaskById(taskId);
        Employee foundChanger = await GetEmployeeFromDbAsync(changerId);

        foundReportsTask.ChangeContent(foundChanger, newContent);
        _dbContext.Update(foundReportsTask);

        await _dbContext.SaveChangesAsync();
    }

    public async void AddComment(Guid taskId, Guid commentatorId, string comment)
    {
        ReportsTask foundReportsTask = await GetTaskById(taskId);
        Employee foundCommentator = await GetEmployeeFromDbAsync(commentatorId);

        foundReportsTask.AddComment(foundCommentator, comment);
        _dbContext.Update(foundReportsTask);

        await _dbContext.SaveChangesAsync();
    }

    public async Task SetOwner(Guid taskId, Guid changerId, Guid newImplementerId)
    {
        ReportsTask foundReportsTask = await GetTaskById(taskId);
        Employee foundChanger = await GetEmployeeFromDbAsync(changerId);
        Employee foundNewOwner = await GetEmployeeFromDbAsync(newImplementerId);

        foundReportsTask.SetOwner(foundChanger, foundNewOwner);
        _dbContext.Update(foundReportsTask);

        await _dbContext.SaveChangesAsync();
    }

    private async Task<bool> IsTaskExist(Guid id) => await _dbContext.Tasks.AnyAsync(t => t.TaskId == id);

    private async Task<Employee> GetEmployeeFromDbAsync(Guid employeeId) =>
        await _dbContext.Employees.SingleAsync(e => e.Id == employeeId);
}