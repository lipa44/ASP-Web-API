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
    public Guid? WorkTeamId { get; protected set; }
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

    public void SetChief(Employee chiefToSet)
    {
        ArgumentNullException.ThrowIfNull(chiefToSet);

        Chief = chiefToSet;
        ChiefId = chiefToSet.Id;
    }

    public void AddSubordinate(Employee subordinate)
    {
        ArgumentNullException.ThrowIfNull(subordinate);

        if (!IsRoleHigherThan(subordinate))
            throw new PermissionDeniedException($"{Role} roles can't add {subordinate.Role}'s as subordinate");

        if (IsSubordinateExist(subordinate))
            throw new ReportsException($"Employee {subordinate} to add already exists in {this}'s subordinates");

        _subordinates.Add(subordinate);
    }

    public void RemoveSubordinate(Employee subordinate)
    {
        ArgumentNullException.ThrowIfNull(subordinate);

        if (!_subordinates.Remove(subordinate))
            throw new ReportsException($"Employee {subordinate} to remove doesn't exist in {this}'s subordinates");
    }

    public void AddTask(ReportsTask taskToAdd)
    {
        ArgumentNullException.ThrowIfNull(taskToAdd);

        if (IsTaskExist(taskToAdd))
            throw new ReportsException($"Task {taskToAdd} already exists in {this}'s tasks");

        _tasks.Add(taskToAdd);
    }

    public void RemoveTask(ReportsTask taskToRemove)
    {
        ArgumentNullException.ThrowIfNull(taskToRemove);

        if (!_tasks.Remove(taskToRemove))
            throw new ReportsException($"Task {taskToRemove} doesn't exist in {this}'s tasks");
    }

    public void SetWorkTeam(WorkTeam workTeamToSet)
    {
        ArgumentNullException.ThrowIfNull(workTeamToSet);

        if (IsWorkTeamSet(workTeamToSet))
            throw new ReportsException($"{this} already has {workTeamToSet} team");

        WorkTeamId = workTeamToSet.Id;
    }

    public void RemoveWorkTeam(WorkTeam workTeamToRemove)
    {
        ArgumentNullException.ThrowIfNull(workTeamToRemove);

        if (!IsWorkTeamSet(workTeamToRemove))
            throw new ReportsException($"{this} doesn't exist in any work team to remove team");

        WorkTeamId = default;
    }

    public bool IsRoleHigherThan(Employee employee) => Role > employee.Role;
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