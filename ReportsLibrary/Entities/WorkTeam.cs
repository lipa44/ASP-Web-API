using System;
using System.Collections.Generic;
using System.Linq;
using ReportsLibrary.Employees;
using ReportsLibrary.Tasks;
using ReportsLibrary.Tools;

namespace ReportsLibrary.Entities
{
    public class WorkTeam
    {
        private readonly List<Sprint> _sprints = new ();
        private readonly List<Employee> _employeesAboba = new ();

        public WorkTeam() { }

        public WorkTeam(Employee teamLead, string name)
        {
            ArgumentNullException.ThrowIfNull(teamLead);
            ReportsException.ThrowIfNullOrWhiteSpace(name);

            TeamLead = teamLead;

            // TeamLeadId = teamLead.Id;
            Name = name;
            Report = new (this);
        }

        public Report Report { get; init; }
        public Employee TeamLead { get; init; }

        public Guid TeamLeadId { get; init; }
        public string Name { get; init; }
        public Guid Id { get; init; } = Guid.NewGuid();

        public virtual ICollection<Employee> EmployeesAboba => _employeesAboba;
        public virtual ICollection<Sprint> Sprints => _sprints;
        public Sprint GetCurrentSprint => _sprints.SingleOrDefault(s => s.ExpirationDate < DateTime.Now)
                                          ?? throw new ReportsException($"No current sprint in {Name} team");
        public IReadOnlyCollection<Employee> GetEmployeesByRole(EmployeeRoles role) =>
            _employeesAboba.Where(e => e.Role == role).ToList();

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

        public void AddEmployee(Employee subordinate)
        {
            ArgumentNullException.ThrowIfNull(subordinate);

            if (IsEmployeeExist(subordinate))
                throw new ReportsException($"Employee to add into {Name} team doesn't exist");

            _employeesAboba.Add(subordinate);
        }

        public void RemoveEmployee(Employee employee)
        {
            ArgumentNullException.ThrowIfNull(employee);

            if (!_employeesAboba.Remove(employee))
                throw new ReportsException($"Employee to remove from {Name} team doesn't exist");
        }

        public void AddTaskToSprint(ReportsTask reportsTask, Sprint sprint)
        {
            ArgumentNullException.ThrowIfNull(reportsTask);
            ArgumentNullException.ThrowIfNull(sprint);

            if (!IsSprintExist(sprint))
                throw new ReportsException($"Sprint in team {Name} to add task doesn't exist");

            sprint.AddTask(reportsTask);
        }

        public void RemoveTaskFromSprint(ReportsTask reportsTask, Sprint sprint)
        {
            ArgumentNullException.ThrowIfNull(reportsTask);
            ArgumentNullException.ThrowIfNull(sprint);

            if (!IsSprintExist(sprint))
                throw new ReportsException($"Sprint in {Name} team to remove task from doesn't exist");

            sprint.RemoveTask(reportsTask);
        }

        public void AddDailyChangesToReport(Employee employee)
        {
            ArgumentNullException.ThrowIfNull(employee);

            // EmployeeTasks(employee).ToList().ForEach(t => _report.AddDailyReport(employee, t));
        }

        public IReadOnlyCollection<ReportsTask> EmployeeTasks(Employee employee) => GetCurrentSprint.Tasks
            .Where(t => t.Owner != null
                        && t.Owner.Equals(employee)).ToList();

        public override string ToString() => Name;

        private bool IsSprintExist(Sprint sprint) => _sprints.Any(s => s.ExpirationDate >= sprint.ExpirationDate);
        private bool IsEmployeeExist(Employee employee) => _employeesAboba.Any(e => e.Equals(employee));
        private bool HasRightsToEdit(Employee changer) => TeamLead.Id == changer.Id;
    }
}