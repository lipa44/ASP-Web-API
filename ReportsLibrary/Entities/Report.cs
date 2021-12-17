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
    public Guid OwnerId { get; init; }
    public WorkTeam WorkTeam { get; private set; }
    public Guid Id { get; init; } = Guid.NewGuid();
    public IReadOnlyCollection<TaskModification> Modifications => _modifications;

    public Report CommitModifications(ICollection<TaskModification> modificationsToCommit)
    {
        ArgumentNullException.ThrowIfNull(modificationsToCommit);

        _modifications.AddRange(GetUncommittedModifications(modificationsToCommit));

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

    private List<TaskModification> GetUncommittedModifications(ICollection<TaskModification> modificationsToCommit) =>
        modificationsToCommit
            .Except(_modifications
                .Select(m => m)).ToList();
}