using System;
using ReportsLibrary.Employees.Abstractions;
using ReportsLibrary.Tools;

namespace ReportsLibrary.Employees
{
    public class OrdinaryEmployee : Subordinate
    {
        public OrdinaryEmployee(string name, string surname, Guid passportId, Employee chief)
            : base(name, surname, passportId)
            => SetChief(chief);

        public override void SetChief(Employee chief)
        {
            ArgumentNullException.ThrowIfNull(chief);

            if (!IsHigherRole(chief))
                throw new PermissionDeniedException($"{chief} has too low a position to become {this}'s a chief");

            Chief = chief;
        }

        public override void AddSubordinate(Subordinate subordinate)
        {
            ArgumentNullException.ThrowIfNull(subordinate);

            if (IsHigherRole(subordinate))
                throw new PermissionDeniedException($"Can't add {subordinate} to subordinates because of role");

            if (IsSubordinateExist(subordinate))
                throw new ReportsException($"Employee {subordinate} already exists in {this}'s subordinates");

            subordinate.SetChief(this);
            Employees.Add(subordinate);
        }

        public override void RemoveSubordinate(Subordinate subordinate)
        {
            ArgumentNullException.ThrowIfNull(subordinate);

            if (!Employees.Remove(subordinate))
                throw new ReportsException($"Employee {subordinate} doesn't exist in {this}'s subordinates");
        }

        public override bool IsHigherRole(Employee employee) => employee is Supervisor or TeamLead;
    }
}