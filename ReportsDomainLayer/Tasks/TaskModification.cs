using System;
using ReportsLibrary.Employees;
using ReportsLibrary.Enums;
using ReportsLibrary.Tools;

namespace ReportsLibrary.Tasks;

public class TaskModification
{
    public TaskModification() { }

    public TaskModification(Employee changer, object data, TaskModificationActions action, DateTime modificationTime)
    {
        // ArgumentNullException.ThrowIfNull(changer);
        // ArgumentNullException.ThrowIfNull(data);
        if (modificationTime == default)
            throw new ReportsException("Task change time can't be default");

        ChangerId = changer.Id;
        Data = data.ToString();
        Action = action;
        ModificationTime = modificationTime;
    }

    public Guid? ChangerId { get; init; }
    public string Data { get; init; }
    public TaskModificationActions Action { get; init; }
    public DateTime ModificationTime { get; init; }
    public Guid Id { get; init; } = Guid.NewGuid();

    public override bool Equals(object obj) => Equals(obj as TaskModification);
    public override int GetHashCode() => HashCode.Combine(Id, Data, Action, ModificationTime, ChangerId);

    private bool Equals(TaskModification taskModification) => taskModification is not null &&
                                                              taskModification.Id == Id
                                                              && taskModification.Action == Action
                                                              && taskModification.ChangerId == ChangerId;
}