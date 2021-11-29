#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using ReportsLibrary.Employees.Abstractions;
using ReportsLibrary.Tasks;

namespace ReportsLibrary.Entities
{
    public class TaskService : ITaskService
    {
        private readonly List<Task> _tasks = new ();

        public Task? FindTaskById(Guid taskId) => _tasks.SingleOrDefault(t => t.Id == taskId);

        public Task GetTaskById(Guid taskId) => _tasks.Single(t => t.Id == taskId);

        public IReadOnlyCollection<Task> FindTasksByCreationTime(DateTime creationTime) =>
            _tasks.Where(t => t.CreationTime == creationTime).ToList();

        public IReadOnlyCollection<Task> FindTasksByModificationTime(DateTime modificationTime) =>
            _tasks.Where(t => t.ModificationTime == modificationTime).ToList();

        public IReadOnlyCollection<Task> FindTaskByEmployee(Employee employee) =>
            _tasks.Where(t => t.Implementer.PassportId == employee.PassportId).ToList();

        public IReadOnlyCollection<Task> FindTasksModifiedByEmployee(Employee employee) =>
            _tasks.Where(t => t.Modifications
                .Any(m => m.Changer.PassportId == employee.PassportId)).ToList();

        public IReadOnlyCollection<Task> FindTasksCreatedByEmployeeSubordinates(Employee employee)
        {
            throw new NotImplementedException();
        }
    }
}