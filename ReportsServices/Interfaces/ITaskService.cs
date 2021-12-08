using System;
using System.Collections.Generic;
using ReportsLibrary.Employees;
using ReportsLibrary.Tasks;
using ReportsLibrary.Tasks.TaskStates;

namespace Reports.Interfaces
{
    public interface ITaskService
    {
        Task? FindTaskById(Guid taskId);
        Task GetTaskById(Guid taskId);
        IReadOnlyCollection<Task?> FindTasksByCreationTime(DateTime creationTime);
        IReadOnlyCollection<Task?> FindTasksByModificationTime(DateTime modificationTime);
        IReadOnlyCollection<Task?> FindTaskByEmployee(Employee employee);
        IReadOnlyCollection<Task?> FindTasksModifiedByEmployee(Employee employee);
        IReadOnlyCollection<Task?> FindTasksCreatedByEmployeeSubordinates(Employee subordinate);
        Task CreateTask(Employee implementor, string taskName);
        void ChangeTaskState(Task task, Employee changer, TaskState newTaskState);
        void ChangeTaskContent(Task task, Employee changer, string newContent);
        void AddComment(Task task, Employee changer, string comment);
        void ChangeImplementor(Task task, Employee changer, Employee newImplementer);
    }
}