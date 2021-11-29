using System;
using System.Collections.Generic;
using ReportsLibrary.Employees.Abstractions;
using ReportsLibrary.Tasks;

namespace ReportsLibrary.Entities
{
    public interface ITaskService
    {
        Task? FindTaskById(Guid taskId);
        Task GetTaskById(Guid taskId);
        IReadOnlyCollection<Task?> FindTasksByCreationTime(DateTime creationTime);
        IReadOnlyCollection<Task?> FindTasksByModificationTime(DateTime modificationTime);
        IReadOnlyCollection<Task?> FindTaskByEmployee(Employee employee);
        IReadOnlyCollection<Task?> FindTasksModifiedByEmployee(Employee employee);
        IReadOnlyCollection<Task?> FindTasksCreatedByEmployeeSubordinates(Employee employee);
    }
}