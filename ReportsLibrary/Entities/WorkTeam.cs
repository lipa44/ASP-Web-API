using System;
using System.Collections.Generic;
using System.Linq;
using ReportsLibrary.Employees;
using ReportsLibrary.Employees.Abstractions;
using ReportsLibrary.Tools;

namespace ReportsLibrary.Entities
{
    public class WorkTeam
    {
        private readonly List<Sprint> _sprints = new ();
        private readonly List<Employee> _employees = new ();

        public WorkTeam(TeamLead teamLead, string name)
        {
            ArgumentNullException.ThrowIfNull(teamLead);
            ReportsException.ThrowIfNullOrWhiteSpace(name);

            TeamLead = teamLead;
            Name = name;
        }

        public IReadOnlyCollection<OrdinaryEmployee> OrdinaryEmployees => _employees.OfType<OrdinaryEmployee>().ToList();
        public IReadOnlyCollection<Supervisor> Supervisors => _employees.OfType<Supervisor>().ToList();
        public IReadOnlyCollection<Employee> Employees => _employees;
        public IReadOnlyCollection<Sprint> Sprints => _sprints;
        public TeamLead TeamLead { get; }
        public string Name { get; }
        public Guid Id { get; } = Guid.NewGuid();

        public void AddSprint(TeamLead changer, Sprint sprint)
        {
            ArgumentNullException.ThrowIfNull(changer);
            ArgumentNullException.ThrowIfNull(sprint);

            if (!HasRightsToEdit(changer))
                throw new PermissionDeniedException($"{changer} don't have access in {Name} team");

            if (IsSprintExist(sprint))
                throw new ReportsException($"Sprint to add into {Name} team until {sprint.ExpirationDate} already exists");

            _sprints.Add(sprint);
        }

        public void RemoveSprint(TeamLead changer, Sprint sprint)
        {
            ArgumentNullException.ThrowIfNull(changer);
            ArgumentNullException.ThrowIfNull(sprint);

            if (!HasRightsToEdit(changer))
                throw new PermissionDeniedException($"{changer} don't have access in {Name} team");

            if (!_sprints.Remove(sprint))
                throw new ReportsException($"Sprint to remove from {Name} team doesn't exist");
        }

        public void AddEmployee(Subordinate subordinate)
        {
            ArgumentNullException.ThrowIfNull(subordinate);

            if (IsEmployeeExist(subordinate))
                throw new ReportsException($"Employee to add into {Name} team doesn't exist");

            _employees.Add(subordinate);
        }

        public void RemoveEmployee(Employee employee)
        {
            ArgumentNullException.ThrowIfNull(employee);

            if (!_employees.Remove(employee))
                throw new ReportsException($"Employee to remove from {Name} team doesn't exist");
        }

        public override string ToString() => Name;

        private bool IsSprintExist(Sprint sprint) => _sprints.Any(s => s.ExpirationDate >= sprint.ExpirationDate);
        private bool IsEmployeeExist(Employee employee) => _employees.Any(e => e.Equals(employee));
        private bool HasRightsToEdit(TeamLead changer) => TeamLead.PassportId == changer.PassportId;
    }
}