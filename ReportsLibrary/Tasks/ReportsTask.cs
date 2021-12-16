using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using ReportsLibrary.Employees;
using ReportsLibrary.Entities;
using ReportsLibrary.Tasks.TaskSnapshots;
using ReportsLibrary.Tasks.TaskStates;
using ReportsLibrary.Tools;

namespace ReportsLibrary.Tasks
{
    public class ReportsTask : ITask
    {
        // private readonly List<TaskSnapshot> _snapshots = new ();
        private List<TaskModification> _modifications = new ();
        private List<TaskComment> _comments = new ();
        private List<Sprint> _sprints = new ();

        public ReportsTask() { }

        public ReportsTask(string title)
        {
            ReportsException.ThrowIfNullOrWhiteSpace(title);

            Title = title;
        }

        public string Title { get; private set; }
        public string Content { get;  private set; }
        public DateTime CreationTime { get; } = DateTime.Now;
        public DateTime ModificationTime { get; private set; } = DateTime.Now;
        public TaskState State { get; private set; } = new OpenTaskState();
        public virtual Employee Owner { get; private set; }
        public Guid? OwnerId { get; private set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid TaskId { get; init; } = Guid.NewGuid();

        // public IReadOnlyCollection<TaskSnapshot> Snapshots => _snapshots;
        public IReadOnlyCollection<TaskModification> Modifications => _modifications;
        public IReadOnlyCollection<TaskComment> Comments => _comments;
        public Guid? SprintId { get; set; }
        public Sprint Sprint { get; set; }

        public void ChangeName(Employee changer, string newName)
        {
            ArgumentNullException.ThrowIfNull(changer);
            ReportsException.ThrowIfNullOrWhiteSpace(newName);

            Title = newName;
            ModificationTime = DateTime.Now;

            _modifications.Add(new (changer, newName, TaskModificationActions.CommentAdded, ModificationTime));
        }

        public void ChangeContent(Employee changer, string newContent)
        {
            ArgumentNullException.ThrowIfNull(changer);
            ReportsException.ThrowIfNullOrWhiteSpace(newContent);

            Content = newContent;
            ModificationTime = DateTime.Now;

            _modifications.Add(new (changer, newContent, TaskModificationActions.ContentChanged, ModificationTime));
        }

        public void AddComment(Employee changer, string comment)
        {
            ArgumentNullException.ThrowIfNull(changer);
            ReportsException.ThrowIfNullOrWhiteSpace(comment);

            _comments.Add(new (changer, comment));
            ModificationTime = DateTime.Now;

            _modifications.Add(new (changer, comment, TaskModificationActions.CommentAdded, ModificationTime));
        }

        public void SetOwner(Employee changer, Employee newImplementer)
        {
            // ArgumentNullException.ThrowIfNull(changer);
            // ArgumentNullException.ThrowIfNull(newImplementer);
            Owner = newImplementer;
            OwnerId = newImplementer.Id;
            ModificationTime = DateTime.Now;

            _modifications.Add(new (changer, newImplementer, TaskModificationActions.ImplementerChanged, ModificationTime));
        }

        public void SetState(Employee changer, TaskState newState)
        {
            ArgumentNullException.ThrowIfNull(changer);
            ArgumentNullException.ThrowIfNull(newState);

            State = newState;
            ModificationTime = DateTime.Now;

            _modifications.Add(new (changer, newState, TaskModificationActions.StateChanged, ModificationTime));
        }

        // public void MakeSnapshot() => _snapshots.Add(new ()
        // {
        //     Name = this.Title, Content = this.Content, ModificationTime = this.ModificationTime,
        //     Owner = this.Owner, TaskState = this.State, Comments = this._comments,
        //     Modifications = this._modifications,
        // });
        //
        // public void RestorePreviousSnapshot() =>
        //     RestoreSnapshot(_snapshots
        //         .LastOrDefault(s => s.ModificationTime < ModificationTime));
        //
        // public void RestoreNextSnapshot() =>
        //     RestoreSnapshot(_snapshots
        //         .FirstOrDefault(s => s.ModificationTime > ModificationTime));
        public override bool Equals(object obj) => Equals(obj as ReportsTask);
        public override int GetHashCode() => HashCode.Combine(TaskId);
        private bool Equals(ReportsTask reportsTask) => reportsTask is not null && reportsTask.TaskId == TaskId;

        private void RestoreSnapshot(ITaskSnapshot snapshot)
        {
            if (snapshot is null)
                throw new ReportsException($"No backup to restore {Title}");

            Title = snapshot.Name;
            Content = snapshot.Content;
            ModificationTime = snapshot.ModificationTime;
            Owner = snapshot.Owner;
            State = snapshot.TaskState;
            _comments = snapshot.Comments;
            _modifications = snapshot.Modifications;
        }
    }
}