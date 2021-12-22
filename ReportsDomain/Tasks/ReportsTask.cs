using System;
using System.Collections.Generic;
using ReportsDomain.Employees;
using ReportsDomain.Entities;
using ReportsDomain.Enums;
using ReportsDomain.Tasks.TaskOperationValidators;
using ReportsDomain.Tasks.TaskOperationValidators.Abstractions;
using ReportsDomain.Tasks.TaskSnapshots;
using ReportsDomain.Tools;

namespace ReportsDomain.Tasks;

public class ReportsTask : ITask
{
    // private readonly List<TaskSnapshot> _snapshots = new ();
    private List<TaskModification> _modifications = new ();
    private List<TaskComment> _comments = new ();

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
    public Enums.TaskStates State { get; private set; } = Enums.TaskStates.Open;
    public virtual Employee Owner { get; private set; }
    public Guid? OwnerId { get; private set; }
    public Guid Id { get; init; } = Guid.NewGuid();

    // public IReadOnlyCollection<TaskSnapshot> Snapshots => _snapshots;
    public IReadOnlyCollection<TaskModification> Modifications => _modifications;
    public IReadOnlyCollection<TaskComment> Comments => _comments;
    public Guid? SprintId { get; private set; }
    public Sprint Sprint { get; private set; }

    public void SetName(Employee changer, string newName)
    {
        ArgumentNullException.ThrowIfNull(changer);
        ReportsException.ThrowIfNullOrWhiteSpace(newName);

        ITaskOperationValidator operationValidator
            = new TaskOperationValidatorFactory().CreateValidator(this);

        if (!operationValidator.HasPermissionToSetTitle(changer))
            throw new PermissionDeniedException($"{changer} don't have permission to set {this}'s task title");

        Title = newName;
        ModificationTime = DateTime.Now;

        _modifications.Add(new (changer.Id, newName, TaskModificationActions.CommentAdded, ModificationTime));
    }

    public void SetContent(Employee changer, string newContent)
    {
        ArgumentNullException.ThrowIfNull(changer);
        ReportsException.ThrowIfNullOrWhiteSpace(newContent);

        ITaskOperationValidator operationValidator
            = new TaskOperationValidatorFactory().CreateValidator(this);

        if (!operationValidator.HasPermissionToSetContent(changer))
            throw new PermissionDeniedException($"{changer} don't have permission to set {this}'s task content");

        Content = newContent;
        ModificationTime = DateTime.Now;

        _modifications.Add(new (changer.Id, newContent, TaskModificationActions.ContentChanged, ModificationTime));
    }

    public void AddComment(Employee changer, string comment)
    {
        ArgumentNullException.ThrowIfNull(changer);
        ReportsException.ThrowIfNullOrWhiteSpace(comment);

        ITaskOperationValidator operationValidator
            = new TaskOperationValidatorFactory().CreateValidator(this);

        if (!operationValidator.HasPermissionToAddComment(changer))
            throw new PermissionDeniedException($"{changer} don't have permission to add {this}'s task comment");

        _comments.Add(new (changer, comment));
        ModificationTime = DateTime.Now;

        _modifications.Add(new (changer.Id, comment, TaskModificationActions.CommentAdded, ModificationTime));
    }

    public void SetOwner(Employee changer, Employee newImplementer)
    {
        ArgumentNullException.ThrowIfNull(changer);
        ArgumentNullException.ThrowIfNull(newImplementer);

        ITaskOperationValidator operationValidator
            = new TaskOperationValidatorFactory().CreateValidator(this);

        if (!operationValidator.HasPermissionToSetOwner(changer))
            throw new PermissionDeniedException($"{changer} don't have permission to set {this}' task owner");

        Owner = newImplementer;
        OwnerId = newImplementer.Id;
        ModificationTime = DateTime.Now;

        _modifications.Add(new (changer.Id, newImplementer, TaskModificationActions.ImplementerChanged, ModificationTime));
    }

    public void SetState(Employee changer, Enums.TaskStates newState)
    {
        ArgumentNullException.ThrowIfNull(changer);
        ArgumentNullException.ThrowIfNull(newState);

        ITaskOperationValidator operationValidator
            = new TaskOperationValidatorFactory().CreateValidator(this);

        if (!operationValidator.HasPermissionToSetState(changer))
            throw new PermissionDeniedException($"{changer} don't have permission to set {this}' task state");

        State = newState;
        ModificationTime = DateTime.Now;

        _modifications.Add(new (changer.Id, newState, TaskModificationActions.StateChanged, ModificationTime));
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
    public override string ToString() => Title;
    public override bool Equals(object obj) => Equals(obj as ReportsTask);
    public override int GetHashCode() => HashCode.Combine(Id);
    private bool Equals(ReportsTask reportsTask) => reportsTask is not null && reportsTask.Id == Id;

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