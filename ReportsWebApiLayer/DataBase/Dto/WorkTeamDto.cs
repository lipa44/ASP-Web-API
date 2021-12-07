using ReportsLibrary.Employees;
using ReportsLibrary.Entities;

namespace ReportsWebApiLayer.DataBase.Dto;

public class WorkTeamDto
{
    private readonly List<Sprint> _sprints;
    private readonly List<Employee> _employees;
    private Report _report;

    public WorkTeamDto(WorkTeam workTeam)
    {
        ArgumentNullException.ThrowIfNull(workTeam);

        TeamLead = workTeam.TeamLead;
        Name = workTeam.Name;
        Id = workTeam.Id;
        _report = workTeam.Report;
        _sprints = workTeam.Sprints.ToList();
        _employees = workTeam.Employees.ToList();
    }

    public TeamLead TeamLead { get; }
    public string Name { get; }
    public Guid Id { get; } = Guid.NewGuid();
}