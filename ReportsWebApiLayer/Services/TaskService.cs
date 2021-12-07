using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using ReportsLibrary.Employees;
using ReportsLibrary.Tasks.TaskStates;
using ReportsWebApiLayer.DataBase;
using ReportsWebApiLayer.Services.Interfaces;
using Task = ReportsLibrary.Tasks.Task;

namespace ReportsWebApiLayer.Services;

public class TaskService : ITaskService
{
    private readonly ReportsDbContext _dbContext;

    public TaskService(ReportsDbContext context)
    {
        _dbContext = context;
    }
    
    public async Task<ActionResult<List<Task>>> GetTasks()
    {
        return await _dbContext.Tasks.ToListAsync();
    }

    public async Task<Task?> FindTaskById(Guid taskId)
    {
        return await _dbContext.Tasks.SingleOrDefaultAsync(t => t.Id == taskId);
    }

    public async Task<Task> GetTaskById(Guid taskId)
    {
        return await _dbContext.Tasks.SingleAsync(t => t.Id == taskId);
    }

    public async void RemoveTaskById(Guid taskId)
    {
        if (!IsTaskExist(taskId).Result)
            throw new Exception("Task to remove doesn't exist");

        Task taskToRemove = _dbContext.Tasks.Single(t => t.Id == taskId);

        _dbContext.Tasks.Remove(taskToRemove);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<IReadOnlyCollection<Task?>> FindTasksByCreationTime(DateTime creationTime)
    {
        return await _dbContext.Tasks
            .Where(t => t.CreationTime == creationTime).ToListAsync();
    }

    public async Task<IReadOnlyCollection<Task?>> FindTasksByModificationTime(DateTime modificationTime)
    {
        return await _dbContext.Tasks
            .Where(t => t.ModificationTime == modificationTime).ToListAsync();
    }

    public async Task<IReadOnlyCollection<Task?>> FindTaskByEmployee(Employee employee)
    {
        return await _dbContext.Tasks
            .Where(t => employee.Equals(t.Implementer)).ToListAsync();
    }

    public async Task<IReadOnlyCollection<Task?>> FindsTaskModifiedByEmployee(Employee employee)
    {
        return await _dbContext.Tasks
            .Where(t => t.Modifications
                .Any(m => m.Changer.Equals(employee))).ToListAsync();
    }

    public async Task<IReadOnlyCollection<Task?>> FindTasksCreatedByEmployeeSubordinates(Employee subordinate)
    {
        return await _dbContext.Tasks
            .Where(t => subordinate.Subordinates
                .Any(s => s.Equals(t.Implementer))).ToListAsync();
    }

    public async Task<Task> CreateTask(Employee implementor, string taskName)
    { 
        Task newTask = new (taskName); 
        EntityEntry<Task> newTaskEntry =  await _dbContext.Tasks.AddAsync(newTask);
        await _dbContext.SaveChangesAsync();

        return newTaskEntry.Entity;
    }

    public void ChangeTaskState(Task task, Employee changer, ITaskState newState)
    {
        throw new NotImplementedException();
    }

    public void ChangeTaskContent(Task task, Employee changer, string newContent)
    {
        throw new NotImplementedException();
    }

    public void AddTaskComment(Task task, Employee changer, string comment)
    {
        throw new NotImplementedException();
    }

    public void ChangeTaskImplementor(Task task, Employee changer, Employee newImplementer)
    {
        throw new NotImplementedException();
    }

    private async Task<bool> IsTaskExist(Guid id) => await _dbContext.Tasks.AnyAsync(t => t.Id == id);
}