#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using Reports.Interfaces;
using ReportsLibrary.Employees;
using ReportsLibrary.Entities;
using ReportsLibrary.Tools;

namespace Reports.Services
{
    public class EmployeeService : IEmployeeService
    {
        private static EmployeeService? _reportsService;
        private readonly List<Employee> _employees = new ();
        private readonly List<WorkTeam> _teams = new ();

        public static EmployeeService GetInstance() => _reportsService is null
            ? _reportsService = new EmployeeService()
            : throw new ReportsException("Reports service can be created only once");

        public void RegisterEmployee(Employee employee)
        {
            ArgumentNullException.ThrowIfNull(employee);

            if (IsEmployeeExist(employee))
                throw new ReportsException($"Employee {employee} already exists in system");

            _employees.Add(employee);
        }

        public void RemoveEmployee(Employee employee)
        {
            ArgumentNullException.ThrowIfNull(employee);

            if (!_employees.Remove(employee))
                throw new ReportsException($"Employee {employee} doesn't exist in system");
        }

        public void RegisterWorkTeam(WorkTeam workTeam)
        {
            ArgumentNullException.ThrowIfNull(workTeam);

            if (IsWorkTeamExist(workTeam))
                throw new ReportsException($"Work team {workTeam} already exists in service");

            // workTeam.TeamLead!.AddWorkTeam(workTeam);
            _teams.Add(workTeam);
        }

        public void RemoveWorkTeam(WorkTeam workTeam)
        {
            ArgumentNullException.ThrowIfNull(workTeam);

            if (!_teams.Remove(workTeam))
                throw new ReportsException($"Work team {workTeam} doesn't exist in service");

            // workTeam.TeamLead!.RemoveWorkTeam(workTeam);
        }

        public Employee GetEmployeeById(Guid id)
        {
            throw new NotImplementedException();
        }

        public Employee? FindEmployeeById(Guid id)
        {
            throw new NotImplementedException();
        }

        public IReadOnlyCollection<Employee> GetEmployees()
        {
            throw new NotImplementedException();
        }

        public void SetChief(Employee subordinate, Employee newChief)
        {
            ArgumentNullException.ThrowIfNull(subordinate);
            ArgumentNullException.ThrowIfNull(newChief);

            subordinate.SetChief(newChief);
            newChief.AddSubordinate(subordinate);
        }

        private bool IsEmployeeExist(Employee employee) => _employees.Any(e => e.Id == employee.Id);
        private bool IsWorkTeamExist(WorkTeam workTeam) => _teams.Any(t => t.Id == workTeam.Id);
    }
}