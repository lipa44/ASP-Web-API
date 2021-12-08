using System;
using System.Collections.Generic;
using System.Linq;
using ReportsLibrary.Entities;
using ReportsLibrary.Tools;

namespace ReportsLibrary.Employees
{
    public class TeamLead : Employee
    {
        private readonly List<WorkTeam> _teams = new ();

        public TeamLead() { }

        public TeamLead(string name, string surname, Guid id)
            : base(name, surname, id)
        { }

        public IReadOnlyCollection<WorkTeam> WorkTeams => _teams;

        public void AddWorkTeam(WorkTeam workTeam)
        {
            ArgumentNullException.ThrowIfNull(workTeam);

            if (IsWorkTeamExist(workTeam))
                throw new ReportsException($"{workTeam} team already exists in {this}'s teams");

            _teams.Add(workTeam);
        }

        public void RemoveWorkTeam(WorkTeam workTeam)
        {
            ArgumentNullException.ThrowIfNull(workTeam);

            if (!_teams.Remove(workTeam))
                throw new ReportsException($"{workTeam} team doesn't exists in {this}'s teams");
        }

        public override void SetChief(Employee chief) { }

        public override void AddSubordinate(Employee subordinate)
        {
            ArgumentNullException.ThrowIfNull(subordinate);

            if (IsLowerOrEqualRole(subordinate))
                throw new PermissionDeniedException($"{this} has too low a position to become {subordinate}'s a chief");

            if (IsSubordinateExist(subordinate))
                throw new ReportsException($"Employee {subordinate} already exists in {this}'s subordinates");

            subordinate.SetChief(this);
            Employees.Add(subordinate);
        }

        public override void RemoveSubordinate(Employee subordinate)
        {
            ArgumentNullException.ThrowIfNull(subordinate);

            if (!Employees.Remove(subordinate))
                throw new ReportsException($"Employee {subordinate} doesn't exist in {this}'s subordinates");
        }

        public override bool IsLowerOrEqualRole(Employee employee) => employee is TeamLead;

        private bool IsWorkTeamExist(WorkTeam workTeam) => _teams.Any(t => t.Id == workTeam.Id);
    }
}