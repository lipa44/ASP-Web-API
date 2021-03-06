using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Entities.Tasks;
using Domain.Enums;
using Domain.Tools;

namespace Domain.Entities;

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
    public Guid? ChiefId { get; private set; }
    public Guid? WorkTeamId { get; private set; }
    public Report Report { get; private set; }

    public IReadOnlyCollection<Employee> Subordinates => _subordinates;
    public IReadOnlyCollection<ReportsTask> Tasks => _tasks;

    public Report CommitChangesToReport()
        => Report?.CommitModifications(_tasks.SelectMany(task => task.Modifications).ToList());

    public Report CreateReport()
    {
        if (Report != null)
            throw new ReportsException($"Report for {this} already created");

        Report = new Report(this);

        return Report;
    }

    public void SetChief(Employee chief)
    {
        ArgumentNullException.ThrowIfNull(chief);

        ChiefId = chief.Id;
    }

    public void RemoveChief() => ChiefId = null;

    public void AddSubordinate(Employee subordinate)
    {
        ArgumentNullException.ThrowIfNull(subordinate);

        if (!IsRoleHigherThan(subordinate))
            throw new PermissionDeniedException($"{Role} roles can't add {subordinate.Role}'s as subordinate");

        if (IsSubordinateExist(subordinate))
            throw new ReportsException($"Employee {subordinate} to add already exists in {this}'s subordinates");

        subordinate.SetChief(this);
    }

    public void RemoveSubordinate(Employee subordinate)
    {
        ArgumentNullException.ThrowIfNull(subordinate);

        if (!_subordinates.Remove(subordinate))
            throw new ReportsException($"Employee {subordinate} to remove doesn't exist in {this}'s subordinates");

        subordinate.RemoveChief();
    }

    public void AddTask(ReportsTask task)
    {
        ArgumentNullException.ThrowIfNull(task);

        if (IsTaskExist(task))
            throw new ReportsException($"Task {task} already exists in {this}'s tasks");

        _tasks.Add(task);
    }

    public void RemoveTask(ReportsTask task)
    {
        ArgumentNullException.ThrowIfNull(task);

        if (!_tasks.Remove(task))
            throw new ReportsException($"Task {task} doesn't exist in {this}'s tasks");
    }

    public void SetWorkTeam(WorkTeam workTeam)
    {
        ArgumentNullException.ThrowIfNull(workTeam);

        if (IsWorkTeamSet(workTeam))
            throw new ReportsException($"{this} already has {workTeam} team");

        WorkTeamId = workTeam.Id;
    }

    public void RemoveWorkTeam() => WorkTeamId = null;

    public bool IsRoleHigherThan(Employee employee) => Role > employee.Role;
    public override string ToString() => $"{Name} {Surname}";
    public override bool Equals(object obj) => Equals(obj as Employee);
    public override int GetHashCode() => HashCode.Combine(Id, Name, Surname);

    protected bool IsSubordinateExist(Employee employee) => Subordinates.Any(e => e.Equals(employee));
    protected bool IsTaskExist(ReportsTask reportsTask) => Tasks.Any(t => t.Equals(reportsTask));
    protected bool IsWorkTeamSet(WorkTeam workTeam) => WorkTeamId != null;

    private bool Equals(Employee employee) => employee is not null && employee.Id == Id
                                                                   && employee.Name == Name
                                                                   && employee.Surname == Surname;
}