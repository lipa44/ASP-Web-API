using System;
using System.Collections.Generic;
using System.Linq;
using Reports.Employees;
using Reports.Employees.Abstractions;
using Reports.Tools;

namespace Reports.Entities
{
    public class ReportsService : IReportsService
    {
        private static ReportsService _reportsService;
        private readonly List<Employee> _employees = new ();
        private readonly List<WorkTeam> _teams = new ();

        public static ReportsService GetInstance() => _reportsService is null
            ? _reportsService = new ReportsService()
            : throw new ReportsException("Isu can be created only once");

        public void RegisterEmployee(Employee employee)
        {
            ArgumentNullException.ThrowIfNull(employee);

            if (IsEmployeeExist(employee))
                throw new ReportsException($"Team lead {employee} already exists in system");

            _employees.Add(employee);
        }

        public void RemoveEmployee(Employee employee)
        {
            ArgumentNullException.ThrowIfNull(employee);

            if (!_employees.Remove(employee))
                throw new ReportsException($"Team lead {employee} doesn't exist in system");
        }

        public void RegisterWorkTeam(WorkTeam workTeam)
        {
            ArgumentNullException.ThrowIfNull(workTeam);

            if (IsWorkTeamExist(workTeam))
                throw new ReportsException($"Work team {workTeam} already exists in service");

            workTeam.Lead.AddWorkTeam(workTeam);
            _teams.Add(workTeam);
        }

        public void RemoveWorkTeam(WorkTeam workTeam)
        {
            ArgumentNullException.ThrowIfNull(workTeam);

            if (!_teams.Remove(workTeam))
                throw new ReportsException($"Work team {workTeam} doesn't exist in service");

            workTeam.Lead.RemoveWorkTeam(workTeam);
        }

        public void ChangeChief(Subordinate subordinate, Subordinate newChief)
        {
            ArgumentNullException.ThrowIfNull(subordinate);
            ArgumentNullException.ThrowIfNull(newChief);

            subordinate.SetChief(newChief);
            newChief.AddSubordinate(subordinate);
        }

        private bool IsEmployeeExist(Employee employee) => _employees.Any(e => e.PassportId == employee.PassportId);
        private bool IsWorkTeamExist(WorkTeam workTeam) => _teams.Any(t => t.Id == workTeam.Id);
    }
}