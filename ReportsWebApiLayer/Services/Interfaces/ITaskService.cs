using Microsoft.AspNetCore.Mvc;
using ReportsLibrary.Employees;
using ReportsLibrary.Tasks.TaskStates;
using Task = ReportsLibrary.Tasks.Task;

namespace ReportsWebApiLayer.Services.Interfaces
{
    public interface ITaskService
    {
        Task<List<Task>> GetTasks();
        Task<Task> FindTaskById(Guid taskId);
        Task<Task> GetTaskById(Guid taskId);
        void RemoveTaskById(Guid taskId);
        Task<IReadOnlyCollection<Task>> FindTasksByCreationTime(DateTime creationTime);
        Task<IReadOnlyCollection<Task>> FindTasksByModificationTime(DateTime modificationTime);
        Task<IReadOnlyCollection<Task>> FindTaskByEmployeeId(Guid employeeId);
        Task<IReadOnlyCollection<Task>> FindsTaskModifiedByEmployeeId(Guid employeeId);
        Task<IReadOnlyCollection<Task>> FindTasksCreatedByEmployeeSubordinates(Guid employeeId);
        Task<Task> CreateTask(string taskName);
        void SetState(Guid taskId, Guid changerId, TaskState newState);
        void SetContent(Guid taskId, Guid changerId, string newContent);
        void AddComment(Guid taskId, Guid changerId, string comment);
        System.Threading.Tasks.Task SetOwner(Guid taskId, Guid changerId, Guid newImplementerId);
    }
}