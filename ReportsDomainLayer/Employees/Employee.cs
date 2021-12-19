using System;
using System.Collections.Generic;
using System.Linq;
using ReportsLibrary.Entities;
using ReportsLibrary.Enums;
using ReportsLibrary.Tasks;
using ReportsLibrary.Tools;

namespace ReportsLibrary.Employees;

public class Employee
{
    private readonly List<ReportsTask> _tasks = new ();
    private readonly List<Employee> _subordinates = new ();

    public Employee() { }

    public Employee(string name, string surname, Guid id, EmployeeRoles role)
    {
        ReportsException.ThrowIfNullOrWhiteSpace(name);
        ReportsException.ThrowIfNullOrWhiteSpace(surname);

        if (id == Guid.Empty)
            throw new ReportsException("Passport ID to create employee is empty");

        Name = name;
        Surname = surname;
        Id = id;
        Role = role;
    }

    public string Name { get; init; }
    public string Surname { get; init; }
    public Guid Id { get; init; }
    public EmployeeRoles Role { get; init; }
    public Employee Chief { get; protected set; }
    public Guid? ChiefId { get; protected set; }
    public WorkTeam WorkTeam { get; protected set; }
    public Guid? WorkTeamId { get; protected set; }
    public Report Report { get; private set; }
    public Guid? ReportId { get; private set; }

    public IReadOnlyCollection<Employee> Subordinates => _subordinates;
    public IReadOnlyCollection<ReportsTask> Tasks => _tasks;

    public void SetChief(Employee chief)
    {
        ArgumentNullException.ThrowIfNull(chief);

        Chief = chief;
        ChiefId = chief.Id;
    }

    public Report CommitChangesToReport()
        => Report?.CommitModifications(_tasks.SelectMany(t => t.Modifications).ToList());

    public Report CreateReport()
    {
        if (Report != null)
            throw new ReportsException($"Report for {this} already created");

        Report = new Report(this);
        ReportId = Report.Id;

        return Report;
    }

    public void AddSubordinate(Employee subordinate)
    {
        ArgumentNullException.ThrowIfNull(subordinate);

        if (IsSubordinateExist(subordinate))
            throw new ReportsException($"Employee {subordinate} already exists in {this}'s subordinates");

        _subordinates.Add(subordinate);
    }

    public void RemoveSubordinate(Employee subordinate)
    {
        ArgumentNullException.ThrowIfNull(subordinate);

        if (!_subordinates.Remove(subordinate))
            throw new ReportsException($"Employee {subordinate} doesn't exist in {this}'s subordinates");
    }

    public void AddTask(ReportsTask reportsTask)
    {
        ArgumentNullException.ThrowIfNull(reportsTask);

        if (IsTaskExist(reportsTask))
            throw new ReportsException($"Task {reportsTask} already exists in {this}'s tasks");

        _tasks.Add(reportsTask);
    }

    public void RemoveTask(ReportsTask reportsTask)
    {
        ArgumentNullException.ThrowIfNull(reportsTask);

        if (!_tasks.Remove(reportsTask))
            throw new ReportsException($"Task {reportsTask} doesn't exist in {this}'s tasks");
    }

    public void SetWorkTeam(WorkTeam workTeam)
    {
        ArgumentNullException.ThrowIfNull(workTeam);

        if (IsWorkTeamSet(workTeam))
            throw new ReportsException($"{this} already in {workTeam}");

        WorkTeam = workTeam;
        WorkTeamId = workTeam.Id;
    }

    public void RemoveWorkTeam(WorkTeam workTeam)
    {
        ArgumentNullException.ThrowIfNull(workTeam);

        if (!IsWorkTeamSet(workTeam))
            throw new ReportsException($"{this} doesn't exist in any work team to remove team");

        WorkTeam = null;
        WorkTeamId = default;
    }

    public override string ToString() => $"{Name} {Surname}";

    public override bool Equals(object obj) => Equals(obj as Employee);
    public override int GetHashCode() => HashCode.Combine(Id, Name, Surname);

    protected bool IsSubordinateExist(Employee employee) => Subordinates.Any(e => e.Equals(employee));
    protected bool IsTaskExist(ReportsTask reportsTask) => Tasks.Any(t => t.Equals(reportsTask));
    protected bool IsWorkTeamSet(WorkTeam workTeam) => WorkTeamId.Equals(workTeam.Id);

    private bool Equals(Employee employee) => employee is not null && employee.Id == Id
                                                                   && employee.Name == Name
                                                                   && employee.Surname == Surname;
}