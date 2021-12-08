using System;
using System.Collections.Generic;
using System.Linq;
using ReportsLibrary.Employees;
using ReportsLibrary.Tasks.TaskSnapshots;
using ReportsLibrary.Tasks.TaskStates;
using ReportsLibrary.Tools;

namespace ReportsLibrary.Tasks
{
    public class Task : ITask
    {
        private readonly List<TaskSnapshot> _snapshots = new ();
        private List<TaskModification> _modifications = new ();
        private List<TaskComment> _comments = new ();

        public Task(string name)
        {
            ReportsException.ThrowIfNullOrWhiteSpace(name);

            Name = name;
        }

        public string Name { get; private set; }
        public string Content { get;  private set; }
        public DateTime CreationTime { get; } = DateTime.Now;
        public DateTime ModificationTime { get; private set; } = DateTime.Now;
        public Employee Implementer { get; private set; }
        public ITaskState TaskState { get; private set; } = new OpenTaskState();
        public Guid Id { get; } = Guid.NewGuid();

        public IReadOnlyCollection<TaskSnapshot> Snapshots => _snapshots;
        public IReadOnlyCollection<TaskModification> Modifications => _modifications;
        public IReadOnlyCollection<TaskComment> Comments => _comments;

        public void ChangeName(Employee changer, string newName)
        {
            ArgumentNullException.ThrowIfNull(changer);
            ReportsException.ThrowIfNullOrWhiteSpace(newName);

            Name = newName;
            ModificationTime = DateTime.Now;

            _modifications.Add(new (changer, newName, TaskChangeActions.CommentAdded, ModificationTime));
        }

        public void ChangeContent(Employee changer, string newContent)
        {
            ArgumentNullException.ThrowIfNull(changer);
            ReportsException.ThrowIfNullOrWhiteSpace(newContent);

            Content = newContent;
            ModificationTime = DateTime.Now;

            _modifications.Add(new (changer, newContent, TaskChangeActions.CommentAdded, ModificationTime));
        }

        public void AddComment(Employee changer, string comment)
        {
            ArgumentNullException.ThrowIfNull(changer);
            ReportsException.ThrowIfNullOrWhiteSpace(comment);

            _comments.Add(new (changer, comment));
            ModificationTime = DateTime.Now;

            _modifications.Add(new (changer, comment, TaskChangeActions.CommentAdded, ModificationTime));
        }

        public void ChangeImplementer(Employee changer, Employee newImplementer)
        {
            ArgumentNullException.ThrowIfNull(changer);
            ArgumentNullException.ThrowIfNull(newImplementer);

            Implementer = newImplementer;
            ModificationTime = DateTime.Now;

            _modifications.Add(new (changer, newImplementer, TaskChangeActions.ImplementerChanged, ModificationTime));
        }

        public void ChangeState(Employee changer, ITaskState newState)
        {
            ArgumentNullException.ThrowIfNull(changer);
            ArgumentNullException.ThrowIfNull(newState);

            TaskState = newState;
            ModificationTime = DateTime.Now;

            _modifications.Add(new (changer, newState, TaskChangeActions.StateChanged, ModificationTime));
        }

        public void MakeSnapshot() => _snapshots.Add(new ()
        {
            Name = this.Name, Content = this.Content, ModificationTime = this.ModificationTime,
            Implementer = this.Implementer, TaskState = this.TaskState, Comments = this._comments,
            Modifications = this._modifications,
        });

        public void RestorePreviousSnapshot() =>
            RestoreSnapshot(_snapshots
                .LastOrDefault(s => s.ModificationTime < ModificationTime));

        public void RestoreNextSnapshot() =>
            RestoreSnapshot(_snapshots
                .FirstOrDefault(s => s.ModificationTime > ModificationTime));

        public override bool Equals(object obj) => Equals(obj as Task);
        public override int GetHashCode() => HashCode.Combine(Id);
        private bool Equals(Task task) => task is not null && task.Id == Id;

        private void RestoreSnapshot(ITaskSnapshot snapshot)
        {
            if (snapshot is null)
                throw new ReportsException($"No backup to restore {Name}");

            Name = snapshot.Name;
            Content = snapshot.Content;
            ModificationTime = snapshot.ModificationTime;
            Implementer = snapshot.Implementer;
            TaskState = snapshot.TaskState;
            _comments = snapshot.Comments;
            _modifications = snapshot.Modifications;
        }
    }
}