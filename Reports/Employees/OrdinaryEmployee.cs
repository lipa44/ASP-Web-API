using System;
using Reports.Employees.Abstractions;
using Reports.Tools;

namespace Reports.Employees
{
    public class OrdinaryEmployee : Subordinate
    {
        public OrdinaryEmployee(string name, string surname, Guid passportId, Employee chief)
            : base(name, surname, passportId)
        {
            SetChief(chief);
        }

        public override void SetChief(Employee chief)
        {
            ArgumentNullException.ThrowIfNull(chief);

            if (chief is OrdinaryEmployee)
                throw new ReportsException("Ordinary employee can't ba a chief");

            Chief = chief;
        }

        public override void AddSubordinate(Subordinate subordinate)
        {
            ArgumentNullException.ThrowIfNull(subordinate);

            if (subordinate is not OrdinaryEmployee)
                throw new ReportsException($"Can't add {subordinate} to subordinates because of role");

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
    }
}