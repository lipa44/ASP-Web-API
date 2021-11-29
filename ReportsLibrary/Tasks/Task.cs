using System;
using System.Collections.Generic;
using System.Linq;
using ReportsLibrary.Employees.Abstractions;
using ReportsLibrary.Tasks.TaskSnapshots;
using ReportsLibrary.Tasks.TaskStates;
using ReportsLibrary.Tools;

namespace ReportsLibrary.Tasks
{
    public class Task : ITask
    {
        private readonly List<TaskSnapshot> _snapshots = new ();
        private List<TaskComment> _comments = new ();
        private List<TaskModification> _modifications = new ();

        public Task(Employee implementer, string taskName)
        {
            ArgumentNullException.ThrowIfNull(implementer);
            ReportsException.ThrowIfNullOrWhiteSpace(taskName);

            Implementer = implementer;
            TaskName = taskName;
            TaskState = new OpenTaskState();

            MakeSnapshot();
        }

        public string TaskName { get; private set; }
        public string? Content { get;  private set; }
        public DateTime CreationTime { get; } = DateTime.Now;
        public DateTime ModificationTime { get; private set; } = DateTime.Now;
        public Employee Implementer { get; private set; }
        public TaskState TaskState { get; private set; }
        public Guid Id { get; } = Guid.NewGuid();

        public IReadOnlyCollection<TaskComment> Comments => _comments;
        public IReadOnlyCollection<TaskModification> Modifications => _modifications;

        public void ChangeContent(Employee changer, string newTaskContent)
        {
            ArgumentNullException.ThrowIfNull(changer);
            ReportsException.ThrowIfNullOrWhiteSpace(newTaskContent);

            if (!TaskState.IsAbleToChangeContent(changer, newTaskContent))
                throw new PermissionDeniedException($"{changer} is not able to change content in task {TaskName}");

            Content = newTaskContent;
            ModificationTime = DateTime.Now;

            _modifications.Add(new (changer, newTaskContent, TaskChanges.ContentChanged, ModificationTime));
            MakeSnapshot();
        }

        public void AddComment(Employee changer, string comment)
        {
            ArgumentNullException.ThrowIfNull(changer);
            ReportsException.ThrowIfNullOrWhiteSpace(comment);

            if (!TaskState.IsAbleToAddComment(changer, comment))
                throw new PermissionDeniedException($"{changer} is not able to add comment into task {TaskName}");

            _comments.Add(new (changer, comment));
            ModificationTime = DateTime.Now;

            _modifications.Add(new (changer, comment, TaskChanges.CommentAdded, ModificationTime));
            MakeSnapshot();
        }

        public void ChangeImplementer(Employee changer, Employee newImplementer)
        {
            ArgumentNullException.ThrowIfNull(changer);
            ArgumentNullException.ThrowIfNull(newImplementer);

            if (!TaskState.IsAbleToAddImplementor(changer, newImplementer))
                throw new PermissionDeniedException($"{changer} is not able to add implementer into task {TaskName}");

            Implementer = newImplementer;
            ModificationTime = DateTime.Now;

            _modifications.Add(new (changer, newImplementer, TaskChanges.ImplementerChanged, ModificationTime));
            MakeSnapshot();
        }

        public void ChangeState(Employee changer, TaskState newTaskState)
        {
            ArgumentNullException.ThrowIfNull(changer);
            ArgumentNullException.ThrowIfNull(newTaskState);

            if (!TaskState.IsAbleToChangeTaskState(changer, newTaskState))
                throw new PermissionDeniedException($"{changer} is not able to change {TaskName}'s task state");

            TaskState = newTaskState;
            ModificationTime = DateTime.Now;

            _modifications.Add(new (changer, newTaskState, TaskChanges.StateChanged, ModificationTime));
            MakeSnapshot();
        }

        public void MakeSnapshot() => _snapshots.Add(new (this, TaskState));

        public void RestorePreviousSnapshot() =>
            RestoreSnapshot(_snapshots.SkipLast(1)
                .LastOrDefault(s => s.GetModificationTime() < ModificationTime));

        public void RestoreNextSnapshot() =>
            RestoreSnapshot(_snapshots
                .FirstOrDefault(s => s.GetModificationTime() > ModificationTime));

        public override bool Equals(object? obj) => Equals(obj as Task);
        public override int GetHashCode() => HashCode.Combine(Id);
        private bool Equals(Task? task) => task is not null && task.Id == Id;

        private void RestoreSnapshot(ITaskSnapshot? snapshot)
        {
            ArgumentNullException.ThrowIfNull(snapshot);

            TaskName = snapshot.GetName();
            Content = snapshot.GetContent();
            ModificationTime = snapshot.GetModificationTime();
            Implementer = snapshot.GetImplementer();
            TaskState = snapshot.GetTaskState();
            _comments = snapshot.GetComments();
            _modifications = snapshot.GetModifications();
        }
    }
}