#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using Reports.Interfaces;
using Reports.TaskOperationValidators;
using Reports.TaskOperationValidators.Abstractions;
using ReportsLibrary.Employees;
using ReportsLibrary.Tasks;
using ReportsLibrary.Tasks.TaskStates;
using ReportsLibrary.Tools;

namespace Reports.Services
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
            _tasks.Where(t => t.Implementer?.Id == employee.Id).ToList();

        public IReadOnlyCollection<Task> FindTasksModifiedByEmployee(Employee employee) =>
            _tasks.Where(t => t.Modifications
                .Any(m => m.Changer!.Id == employee.Id)).ToList();

        public IReadOnlyCollection<Task> FindTasksCreatedByEmployeeSubordinates(Employee subordinate)
            => _tasks.Where(t => subordinate.Subordinates
                .Any(s => s.Id == t.Implementer?.Id)).ToList();

        public Task CreateTask(Employee implementor, string taskName)
        {
            ArgumentNullException.ThrowIfNull(implementor);
            ReportsException.ThrowIfNullOrWhiteSpace(taskName);

            Task newTask = new (taskName);

            if (!IsTaskExist(newTask))
                throw new ReportsException($"Task {newTask.Name} doesn't exist in system");

            _tasks.Add(newTask);

            return newTask;
        }

        public void ChangeTaskState(Task task, Employee changer, ITaskState newTaskState)
        {
            ArgumentNullException.ThrowIfNull(task);
            ArgumentNullException.ThrowIfNull(changer);
            ArgumentNullException.ThrowIfNull(newTaskState);

            if (!IsTaskExist(task))
                throw new ReportsException($"Task {task.Name} doesn't exist in system");

            ITaskOperationValidator operationValidation = new TaskOperationValidatorFactory().CreateValidator(task);

            if (!operationValidation.HasPermissionToChangeState(changer))
                throw new PermissionDeniedException($"{changer} is not able to change {task.Name}'s task state");

            if (task.TaskState == newTaskState)
                throw new ReportsException($"Task state is already set on {newTaskState}");

            task.ChangeState(changer, newTaskState);
        }

        public void ChangeTaskContent(Task task, Employee changer, string newContent)
        {
            ArgumentNullException.ThrowIfNull(task);
            ArgumentNullException.ThrowIfNull(changer);
            ReportsException.ThrowIfNullOrWhiteSpace(newContent);

            if (!IsTaskExist(task))
                throw new ReportsException($"Task {task.Name} doesn't exist in system");

            ITaskOperationValidator operationValidation = new TaskOperationValidatorFactory().CreateValidator(task);

            if (!operationValidation.HasPermissionToContent(changer))
                throw new PermissionDeniedException($"{changer} is not able to change {task.Name}'s task content");

            task.ChangeContent(changer, newContent);
        }

        public void AddComment(Task task, Employee changer, string comment)
        {
            ArgumentNullException.ThrowIfNull(task);
            ArgumentNullException.ThrowIfNull(changer);
            ReportsException.ThrowIfNullOrWhiteSpace(comment);

            if (!IsTaskExist(task))
                throw new ReportsException($"Task {task.Name} doesn't exist in system");

            ITaskOperationValidator operationValidation = new TaskOperationValidatorFactory().CreateValidator(task);

            if (!operationValidation.HasPermissionToAddComment(changer))
                throw new PermissionDeniedException($"{changer} is not able to add comment to {task.Name}'s task");

            task.AddComment(changer, comment);
        }

        public void ChangeImplementor(Task task, Employee changer, Employee newImplementer)
        {
            ArgumentNullException.ThrowIfNull(task);
            ArgumentNullException.ThrowIfNull(changer);
            ArgumentNullException.ThrowIfNull(newImplementer);

            if (!IsTaskExist(task))
                throw new ReportsException($"Task {task.Name} doesn't exist in system");

            ITaskOperationValidator operationValidation = new TaskOperationValidatorFactory().CreateValidator(task);

            if (!operationValidation.HasPermissionToChangeImplementer(changer))
                throw new PermissionDeniedException($"{changer} is not able to change implementor in {task.Name}'s task");

            task.ChangeImplementer(changer, newImplementer);
        }

        private bool IsTaskExist(Task task) => _tasks.Any(t => t.Id == task.Id);
    }
}