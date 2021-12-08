using Microsoft.AspNetCore.Mvc;
using ReportsLibrary.Employees;
using ReportsLibrary.Tasks.TaskStates;
using Task = ReportsLibrary.Tasks.Task;

namespace ReportsWebApiLayer.Services.Interfaces
{
    public interface ITaskService
    {
        List<Task> GetTasks();
        Task<Task?> FindTaskById(Guid taskId);
        Task<Task> GetTaskById(Guid taskId);
        void RemoveTaskById(Guid taskId);
        Task<IReadOnlyCollection<Task?>> FindTasksByCreationTime(DateTime creationTime);
        Task<IReadOnlyCollection<Task?>> FindTasksByModificationTime(DateTime modificationTime);
        Task<IReadOnlyCollection<Task?>> FindTaskByEmployee(Employee employee);
        Task<IReadOnlyCollection<Task?>> FindsTaskModifiedByEmployee(Employee employee);
        Task<IReadOnlyCollection<Task?>> FindTasksCreatedByEmployeeSubordinates(Employee subordinate);
        Task<Task> CreateTask(string taskName);
        void ChangeTaskState(Task task, Employee changer, TaskState newState);
        void ChangeTaskContent(Task task, Employee changer, string newContent);
        void AddTaskComment(Task task, Employee changer, string comment);
        void ChangeTaskImplementor(Task task, Employee changer, Employee newImplementer);
    }
}