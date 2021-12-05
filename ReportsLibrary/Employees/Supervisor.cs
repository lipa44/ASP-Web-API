using System;
using ReportsLibrary.Tools;

namespace ReportsLibrary.Employees
{
    public class Supervisor : Employee
    {
        public Supervisor(string name, string surname, Guid passportId)
            : base(name, surname, passportId)
        { }

        public sealed override void SetChief(Employee chief)
        {
            ArgumentNullException.ThrowIfNull(chief);

            if (!IsLowerOrEqualRole(chief))
                throw new PermissionDeniedException($"{chief} has too low a position to become {this}'s a chief");

            Chief = chief;
        }

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

        public override bool IsLowerOrEqualRole(Employee employee) => employee is OrdinaryEmployee or Supervisor;
    }
}