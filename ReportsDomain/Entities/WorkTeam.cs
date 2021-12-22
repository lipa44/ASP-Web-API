using System;
using System.Collections.Generic;
using System.Linq;
using ReportsDomain.Employees;
using ReportsDomain.Tasks;
using ReportsDomain.Tools;

namespace ReportsDomain.Entities;

public class WorkTeam
{
    private readonly List<Sprint> _sprints = new ();
    private readonly List<Report> _reports = new ();
    private readonly List<Employee> _employees = new ();

    public WorkTeam() { }

    public WorkTeam(Employee teamLead, string name)
    {
        ArgumentNullException.ThrowIfNull(teamLead);
        ReportsException.ThrowIfNullOrWhiteSpace(name);

        TeamLead = teamLead;
        TeamLeadId = teamLead.Id;
        Name = name;
    }

    public virtual IReadOnlyCollection<Report> Reports => _reports;
    public Employee TeamLead { get; init; }
    public Guid? TeamLeadId { get; init; }
    public string Name { get; init; }
    public Guid Id { get; init; } = Guid.NewGuid();

    public ICollection<Employee> Employees => _employees;
    public virtual ICollection<Sprint> Sprints => _sprints;

    public Sprint GetCurrentSprint => _sprints.SingleOrDefault(s => s.ExpirationDate < DateTime.Now)
                                      ?? throw new ReportsException($"No current sprint in {Name} team");

    public void AddSprint(Employee changer, Sprint sprint)
    {
        ArgumentNullException.ThrowIfNull(changer);
        ArgumentNullException.ThrowIfNull(sprint);

        if (!HasRightsToEdit(changer))
            throw new PermissionDeniedException($"{changer} don't have access in {Name} team");

        if (IsSprintExist(sprint))
            throw new ReportsException($"Sprint to add into {Name} team until {sprint.ExpirationDate} already exists");

        _sprints.Add(sprint);
    }

    public void RemoveSprint(Employee changer, Sprint sprint)
    {
        ArgumentNullException.ThrowIfNull(changer);
        ArgumentNullException.ThrowIfNull(sprint);

        if (!HasRightsToEdit(changer))
            throw new PermissionDeniedException($"{changer} don't have access in {Name} team");

        if (!_sprints.Remove(sprint))
            throw new ReportsException($"Sprint to remove from {Name} team doesn't exist");
    }

    public void AddEmployee(Employee subordinate, Employee changer)
    {
        ArgumentNullException.ThrowIfNull(subordinate);

        if (!HasRightsToEdit(changer))
            throw new PermissionDeniedException($"Only {TeamLead} has permission to add employees into team {Name}");

        if (IsEmployeeExist(subordinate))
            throw new ReportsException($"Employee to add into {Name} team doesn't exist");

        _employees.Add(subordinate);
    }

    public void RemoveEmployee(Employee employee, Employee changer)
    {
        ArgumentNullException.ThrowIfNull(employee);

        if (!HasRightsToEdit(changer))
            throw new PermissionDeniedException($"Only {TeamLead} has permission to remove employees from team {Name}");

        if (!_employees.Remove(employee))
            throw new ReportsException($"Employee to remove from {Name} team doesn't exist");
    }

    public Report GenerateReport(Employee changer)
    {
        ArgumentNullException.ThrowIfNull(changer);
        ArgumentNullException.ThrowIfNull(TeamLead.Report);

        if (!HasRightsToEdit(changer))
            throw new PermissionDeniedException($"Only {TeamLead} has permission to generate {Name}'s team report");

        Employees.ToList()
            .ForEach(e => new ReportsMerger(TeamLead.Report, e.Report).Merge());

        _reports.Add(TeamLead.Report.DeepCloneWithoutOwner());

        return TeamLead.Report;
    }

    public IReadOnlyCollection<ReportsTask> CurrentSprintEmployeeTasks(Employee employee)
        => GetCurrentSprint.Tasks
            .Where(t => t.Owner != null
                        && t.Owner.Equals(employee)).ToList();

    public IReadOnlyCollection<ReportsTask> AllEmployeeTasks(Employee employee)
        => _sprints.SelectMany(t => t.Tasks)
            .Where(t => t.Owner != null
                        && t.Owner.Equals(employee)).ToList();

    public override string ToString() => Name;

    private bool IsSprintExist(Sprint sprint) => _sprints.Any(s => s.ExpirationDate >= sprint.ExpirationDate);
    private bool IsEmployeeExist(Employee employee) => _employees.Any(e => e.Equals(employee));
    private bool HasRightsToEdit(Employee changer) => TeamLead.Id == changer.Id;
}