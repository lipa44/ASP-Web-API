using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using ReportsLibrary.Employees;
using ReportsLibrary.Tasks.TaskStates;
using ReportsWebApiLayer.DataBase;
using ReportsWebApiLayer.Services.Interfaces;
using ReportsTask = ReportsLibrary.Tasks.Task;

namespace ReportsWebApiLayer.Services;

public class TaskService : ITaskService
{
    private readonly ReportsDbContext _dbContext;

    public TaskService(ReportsDbContext context)
    {
        _dbContext = context;
    }

    public Task<List<ReportsTask>> GetTasks() => _dbContext.Tasks.ToListAsync();

    public async Task<ReportsTask> FindTaskById(Guid taskId) =>
        await _dbContext.Tasks.SingleOrDefaultAsync(t => t.Id == taskId);

    public async Task<ReportsTask> GetTaskById(Guid taskId) =>
        await _dbContext.Tasks.SingleAsync(t => t.Id == taskId);

    public async void RemoveTaskById(Guid taskId)
    {
        if (!IsTaskExist(taskId).Result)
            throw new Exception("Task to remove doesn't exist");

        ReportsTask taskToRemove = await _dbContext.Tasks.SingleAsync(t => t.Id == taskId);

        _dbContext.Tasks.Remove(taskToRemove);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<IReadOnlyCollection<ReportsTask>> FindTasksByCreationTime(DateTime creationTime)
    {
        return await _dbContext.Tasks
            .Where(t => t.CreationTime == creationTime).ToListAsync();
    }

    public async Task<IReadOnlyCollection<ReportsTask>> FindTasksByModificationTime(DateTime modificationTime)
    {
        return await _dbContext.Tasks
            .Where(t => t.ModificationTime == modificationTime).ToListAsync();
    }

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

        return await _dbContext.Tasks
            .Where(t => foundEmployee.Subordinates
                .Any(s => s.Equals(t.Owner))).ToListAsync();
    }

    public async Task<ReportsTask> CreateTask(string taskName)
    {
        EntityEntry<ReportsTask> newTaskEntry = await _dbContext.Tasks.AddAsync(new (taskName));

        await _dbContext.SaveChangesAsync();

        return newTaskEntry.Entity;
    }

    public void SetState(Guid taskId, Guid changerId, TaskState newState)
    {
        throw new NotImplementedException();
    }

    public async void SetContent(Guid taskId, Guid changerId, string newContent)
    {
        ReportsTask foundTask = await GetTaskById(taskId);
        Employee foundChanger = await GetEmployeeFromDbAsync(changerId);

        foundTask.ChangeContent(foundChanger, newContent);

        _dbContext.Tasks.Update(foundTask);

        await _dbContext.SaveChangesAsync();
    }

    public void AddComment(Guid taskId, Guid changerId, string comment)
    {
        throw new NotImplementedException();
    }

    public async Task SetOwner(Guid taskId, Guid changerId, Guid newImplementerId)
    {
        ReportsTask foundTask = await GetTaskById(taskId);
        Employee foundChanger = await GetEmployeeFromDbAsync(changerId);
        Employee foundNewOwner = await GetEmployeeFromDbAsync(newImplementerId);

        foundTask.SetOwner(foundChanger, foundNewOwner);
        foundNewOwner.AddTask(foundTask);

        _dbContext.Employees.Update(foundChanger);
        _dbContext.Tasks.Update(foundTask);

        await _dbContext.SaveChangesAsync();
    }

    private async Task<bool> IsTaskExist(Guid id) => await _dbContext.Tasks.AnyAsync(t => t.Id == id);

    private async Task<Employee> GetEmployeeFromDbAsync(Guid employeeId) =>
        await _dbContext.Employees.SingleAsync(e => e.Id == employeeId);
}