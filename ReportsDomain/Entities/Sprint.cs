namespace ReportsDomain.Entities;

using System;
using System.Collections.Generic;
using System.Linq;
using Tasks;
using Tools;

public class Sprint
{
    private readonly List<ReportsTask> _tasks = new ();

    public Sprint() { }

    public Sprint(DateTime expirationDate)
    {
        if (expirationDate == default)
            throw new ReportsException("Sprint's expiration date must be not default");

        ExpirationDate = expirationDate;
    }

    public DateTime ExpirationDate { get; init; }
    public Guid SprintId { get; init; } = Guid.NewGuid();
    public virtual ICollection<ReportsTask> Tasks => _tasks;

    public void AddTask(ReportsTask reportsTask)
    {
        ArgumentNullException.ThrowIfNull(reportsTask);

        if (IsTaskExist(reportsTask))
            throw new ReportsException("Task to add into sprint already exists");

        _tasks.Add(reportsTask);
    }

    public void RemoveTask(ReportsTask reportsTask)
    {
        ArgumentNullException.ThrowIfNull(reportsTask);

        if (!_tasks.Remove(reportsTask))
            throw new ReportsException("Task to remove from sprint doesn't exist");
    }

    public override bool Equals(object obj) => Equals(obj as Sprint);
    public override int GetHashCode() => HashCode.Combine(SprintId);
    private bool Equals(Sprint sprint) => sprint is not null && sprint.SprintId == SprintId;

    private bool IsTaskExist(ReportsTask reportsTask) => _tasks.Any(t => t.Equals(reportsTask));
}