#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using Reports.Employees.Abstractions;
using Reports.Task.TaskSnapshots;
using Reports.Task.TaskStates;
using Reports.Tools;

namespace Reports.Task
{
    public class Task : ITask
    {
        private readonly List<TaskSnapshot> _snapshots = new ();
        private List<TaskComment> _comments = new ();
        private List<Employee> _implementers = new ();
        private TaskState _taskState = new OpenTaskState();

        public Task(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ReportsException("Name to create task is null");

            Name = name;

            _taskState.SetTask(this);
            MakeSnapshot();
        }

        public string Name { get; private set; }
        public string? Content { get;  private set; }
        public DateTime CreationTime { get; } = DateTime.Now;
        public DateTime ModificationTime { get; private set; } = DateTime.Now;
        public Guid Id { get; } = Guid.NewGuid();
        public IReadOnlyCollection<TaskComment> Comments => _comments;
        public IReadOnlyCollection<Employee> Implementers => _implementers;
        public TaskState TaskState => _taskState.Clone();

        public void ChangeName(Employee changer, string newTaskName)
        {
            ArgumentNullException.ThrowIfNull(changer);

            if (string.IsNullOrWhiteSpace(newTaskName))
                throw new ReportsException($"New task name to set in task {Name} is empty");

            if (!_taskState.IsAbleToChangeName(changer, newTaskName))
                throw new PermissionDeniedException($"{changer} is not able to change name in task {Name}");

            Name = newTaskName;
            ModificationTime = DateTime.Now;
            MakeSnapshot();
        }

        public void ChangeContent(Employee changer, string newTaskContent)
        {
            ArgumentNullException.ThrowIfNull(changer);

            if (string.IsNullOrWhiteSpace(newTaskContent))
                throw new ReportsException($"New task content to set in task {Name} is empty");

            if (!_taskState.IsAbleToChangeContent(changer, newTaskContent))
                throw new PermissionDeniedException($"{changer} is not able to change content in task {Name}");

            Content = newTaskContent;
            ModificationTime = DateTime.Now;
            MakeSnapshot();
        }

        public void AddComment(Employee changer, string comment)
        {
            ArgumentNullException.ThrowIfNull(changer);

            if (string.IsNullOrWhiteSpace(comment))
                throw new ReportsException($"Comment to set in task {Name} is empty");

            if (!_taskState.IsAbleToAddComment(changer, comment))
                throw new PermissionDeniedException($"{changer} is not able to add comment into task {Name}");

            _comments.Add(new (changer, comment));
            ModificationTime = DateTime.Now;
            MakeSnapshot();
        }

        public void AddImplementor(Employee changer, Employee newImplementor)
        {
            ArgumentNullException.ThrowIfNull(changer);
            ArgumentNullException.ThrowIfNull(newImplementor);

            if (!_taskState.IsAbleToAddImplementor(changer, newImplementor))
                throw new PermissionDeniedException($"{changer} is not able to add implementor into task {Name}");

            if (IsImplementorExist(newImplementor))
                throw new ReportsException($"Implementor {newImplementor} already exist in task {Name}");

            _implementers.Add(newImplementor);
            ModificationTime = DateTime.Now;
            MakeSnapshot();
        }

        public void ChangeState(Employee changer, TaskState newTaskState)
        {
            ArgumentNullException.ThrowIfNull(changer);
            ArgumentNullException.ThrowIfNull(newTaskState);

            if (newTaskState == _taskState)
                throw new ReportsException($"Current task state is already set on {_taskState}");

            if (!_taskState.IsAbleToChangeTaskState(changer, newTaskState))
                throw new PermissionDeniedException($"{changer} is not able to change {Name}'s task state");

            _taskState = newTaskState.SetTask(this);
            ModificationTime = DateTime.Now;
            MakeSnapshot();
        }

        public void MakeSnapshot() => _snapshots.Add(new (this, _taskState));

        public void RestorePreviousSnapshot() =>
            RestoreSnapshot(_snapshots
                .SkipLast(1)
                .Last(s => s.GetModificationTime() < ModificationTime));

        public void RestoreNextSnapshot() =>
            RestoreSnapshot(_snapshots
                .FirstOrDefault(s => s.GetModificationTime() > ModificationTime));

        public override bool Equals(object? obj) => Equals(obj as Task);
        public override int GetHashCode() => HashCode.Combine(Id);
        private bool Equals(Task? task) => task is not null && task.Id == Id;

        private bool IsImplementorExist(Employee implementor) => _implementers.Any(i => i.Equals(implementor));
        private void RestoreSnapshot(ITaskSnapshot? snapshot)
        {
            if (snapshot is null)
                throw new ReportsException("No snapshots to revert");

            Name = snapshot.GetName();
            Content = snapshot.GetContent();
            ModificationTime = snapshot.GetModificationTime();
            _comments = snapshot.GetComments();
            _implementers = snapshot.GetImplementors();
            _taskState = snapshot.GetTaskState();
        }
    }
}