namespace ReportsDomain.Tasks;

using System;
using System.Collections.Generic;
using Entities;
using Enums;
using TaskOperationValidators;
using TaskOperationValidators.Abstractions;
using TaskSnapshots;
using Tools;

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

    public ReportsTask(string title, Guid ownerId)
    {
        ReportsException.ThrowIfNullOrWhiteSpace(title);

        Title = title;
        OwnerId = ownerId;
    }

    public string Title { get; private set; }
    public string Content { get;  private set; }
    public DateTime CreationTime { get; } = DateTime.Now;
    public DateTime ModificationTime { get; private set; } = DateTime.Now;
    public ReportTaskStates State { get; private set; } = ReportTaskStates.Open;
    public Guid? OwnerId { get; private set; }
    public Guid Id { get; init; } = Guid.NewGuid();

    // public IReadOnlyCollection<TaskSnapshot> Snapshots => _snapshots;
    public IReadOnlyCollection<TaskModification> Modifications => _modifications;
    public IReadOnlyCollection<TaskComment> Comments => _comments;
    public Guid? SprintId { get; private set; }

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

        _modifications.Add(new (changer.Id, changer.ToString(), newName, TaskModificationActions.CommentAdded, ModificationTime));
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

        _modifications.Add(new (changer.Id, changer.ToString(), newContent, TaskModificationActions.ContentChanged, ModificationTime));
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

        _modifications.Add(new (changer.Id, changer.ToString(), comment, TaskModificationActions.CommentAdded, ModificationTime));
    }

    public void SetOwner(Employee changer, Employee newImplementer)
    {
        ArgumentNullException.ThrowIfNull(changer);
        ArgumentNullException.ThrowIfNull(newImplementer);

        ITaskOperationValidator operationValidator
            = new TaskOperationValidatorFactory().CreateValidator(this);

        if (!operationValidator.HasPermissionToSetOwner(changer))
            throw new PermissionDeniedException($"{changer} don't have permission to set {this}' task owner");

        OwnerId = newImplementer.Id;
        ModificationTime = DateTime.Now;

        _modifications.Add(new (changer.Id, changer.ToString(), newImplementer, TaskModificationActions.ImplementerChanged, ModificationTime));
    }

    public void SetState(Employee changer, ReportTaskStates newState)
    {
        ArgumentNullException.ThrowIfNull(changer);
        ArgumentNullException.ThrowIfNull(newState);

        ITaskOperationValidator operationValidator
            = new TaskOperationValidatorFactory().CreateValidator(this);

        if (!operationValidator.HasPermissionToSetState(changer))
            throw new PermissionDeniedException($"{changer} don't have permission to set {this}' task state");

        State = newState;
        ModificationTime = DateTime.Now;

        _modifications.Add(new (changer.Id, changer.ToString(), newState, TaskModificationActions.StateChanged, ModificationTime));
    }

    public void SetSprint(Employee changer, Sprint sprint)
    {
        ArgumentNullException.ThrowIfNull(changer);
        ArgumentNullException.ThrowIfNull(sprint);

        if (changer.Id != OwnerId)
            throw new PermissionDeniedException($"{changer} don't have permission to set {this}' task state");

        SprintId = sprint.Id;
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
        State = snapshot.ReportTaskState;
        _comments = snapshot.Comments;
        _modifications = snapshot.Modifications;
    }
}