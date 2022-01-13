using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Enums;
using Domain.Tasks;
using Domain.Tools;

namespace Domain.Entities;

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
    public ReportStates States { get; private set; }
    public IReadOnlyCollection<TaskModification> Modifications => _modifications;

    public Report CommitModifications(ICollection<TaskModification> modificationsToCommit)
    {
        ArgumentNullException.ThrowIfNull(modificationsToCommit);

        if (States == ReportStates.Done)
            throw new PermissionDeniedException($"Can't commit into report {Id}, because it's state is done");

        _modifications.AddRange(GetUncommittedModifications(modificationsToCommit));

        return this;
    }

    public Report SetReportAsDone(Employee changer)
    {
        ArgumentNullException.ThrowIfNull(changer);

        if (changer.Id != OwnerId)
            throw new PermissionDeniedException($"{changer} has not permission to set report {Id} as done");

        States = ReportStates.Done;

        return this;
    }

    public Report DeepClone()
        => new Report().CommitModifications(Modifications
            .Select(m =>
                new TaskModification(m.ChangerId, m.ChangerData, m.Data, m.Action, m.ModificationTime)).ToList());

    private List<TaskModification> GetUncommittedModifications(ICollection<TaskModification> modificationsToCommit)
        => modificationsToCommit
            .Except(_modifications
                .Select(m => m)).ToList();
}