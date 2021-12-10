using System;

namespace ReportsLibrary.Entities;

public class Report
{
    public Report() { }

    public Report(WorkTeam workTeam)
    {
        WorkTeamName = workTeam.Name;
    }

    public string WorkTeamName { get; init; }
    public Guid Id { get; init; } = Guid.NewGuid();
}