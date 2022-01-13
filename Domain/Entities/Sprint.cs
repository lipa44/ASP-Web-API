using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Tasks;
using Domain.Tools;

namespace Domain.Entities;

public class Sprint
{
    private readonly List<ReportsTask> _tasks = new ();

    public Sprint() { }

    public Sprint(DateTime expirationDate, Guid workTeamId)
    {
        if (expirationDate == default)
            throw new ReportsException("Sprint's expiration date must be not default");

        ExpirationDate = expirationDate;
        WorkTeamId = workTeamId;
    }

    public DateTime ExpirationDate { get; init; }
    public Guid Id { get; init; } = Guid.NewGuid();
    public Guid WorkTeamId { get; init; } = Guid.NewGuid();
    public ICollection<ReportsTask> Tasks => _tasks;

    public void AddTask(ReportsTask reportsTask)
    {
        ArgumentNullException.ThrowIfNull(reportsTask);

        if (IsTaskExist(reportsTask))
            throw new ReportsException("Task to add into sprint already exists");

        // reportsTask.SetSprint(this);
        _tasks.Add(reportsTask);
    }

    public void RemoveTask(ReportsTask reportsTask)
    {
        ArgumentNullException.ThrowIfNull(reportsTask);

        if (!_tasks.Remove(reportsTask))
            throw new ReportsException("Task to remove from sprint doesn't exist");
    }

    public override bool Equals(object obj) => Equals(obj as Sprint);
    public override int GetHashCode() => HashCode.Combine(Id);
    private bool Equals(Sprint sprint) => sprint is not null && sprint.Id == Id;

    private bool IsTaskExist(ReportsTask reportsTask) => _tasks.Any(t => t.Equals(reportsTask));
}