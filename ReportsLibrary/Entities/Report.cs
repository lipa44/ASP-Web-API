using System;
using System.Collections.Generic;
using System.Linq;
using ReportsLibrary.Employees;
using ReportsLibrary.Tasks;

namespace ReportsLibrary.Entities;

public class Report
{
    private readonly List<TaskModification> _modifications = new ();

    public Report() { }

    public Report(Employee owner)
    {
        ArgumentNullException.ThrowIfNull(owner);

        Owner = owner;
    }

    public Employee Owner { get; init; }
    public WorkTeam WorkTeam { get; private set; }
    public Guid Id { get; init; } = Guid.NewGuid();

    public Report CommitChanges(ICollection<ReportsTask> tasksToCommit)
    {
        ArgumentNullException.ThrowIfNull(tasksToCommit);

        _modifications.AddRange(GetUncommittedModifications(tasksToCommit));

        return this;
    }

    // public void SetWorkTeam(WorkTeam workTeam)
    // {
    //     ArgumentNullException.ThrowIfNull(workTeam);
    //
    //     WorkTeam = workTeam;
    //     WorkTeamId = workTeam.Id;
    // }
    private List<TaskModification> GetUncommittedModifications(ICollection<ReportsTask> tasksToCommit) =>
        tasksToCommit.SelectMany(t => t.Modifications)
            .Except(_modifications
                .Select(m => m)).ToList();
}