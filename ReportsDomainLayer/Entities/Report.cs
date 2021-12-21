using System;
using System.Collections.Generic;
using System.Linq;
using ReportsLibrary.Employees;
using ReportsLibrary.Tasks;
using ReportsLibrary.Tools;

namespace ReportsLibrary.Entities;

public class Report
{
    private readonly List<TaskModification> _modifications = new ();

    public Report() { }

    public Report(Employee owner)
    {
        ArgumentNullException.ThrowIfNull(owner);
        Owner = owner;
        OwnerId = owner.Id;
    }

    public Employee Owner { get; init; }
    public Guid OwnerId { get; init; }
    public Guid Id { get; init; } = Guid.NewGuid();
    public bool IsReportDone { get; private set; }
    public IReadOnlyCollection<TaskModification> Modifications => _modifications;

    public Report CommitModifications(ICollection<TaskModification> modificationsToCommit)
    {
        ArgumentNullException.ThrowIfNull(modificationsToCommit);

        if (IsReportDone)
            throw new PermissionDeniedException($"Can't commit into report {Id}, because it's marked as done");

        _modifications.AddRange(GetUncommittedModifications(modificationsToCommit));

        return this;
    }

    public Report SetReportAsDone(Employee changer)
    {
        ArgumentNullException.ThrowIfNull(changer);

        if (changer.Id != OwnerId)
            throw new PermissionDeniedException($"{changer} has not permission to set report {Id} as done");

        IsReportDone = true;

        return this;
    }

    public Report DeepCloneWithoutOwner()
        => new Report().CommitModifications(Modifications
            .Select(m =>
                new TaskModification(m.ChangerId, m.Data, m.Action, m.ModificationTime)).ToList());

    private List<TaskModification> GetUncommittedModifications(ICollection<TaskModification> modificationsToCommit) =>
        modificationsToCommit
            .Except(_modifications
                .Select(m => m)).ToList();
}