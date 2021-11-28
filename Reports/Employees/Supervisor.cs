using System;
using Reports.Employees.Abstractions;
using Reports.Tools;

namespace Reports.Employees
{
    public class Supervisor : Subordinate
    {
        public Supervisor(string name, string surname, Guid passportId, Employee chief)
            : base(name, surname, passportId)
            => SetChief(chief);

        public Supervisor(Subordinate subordinate)
            : base(subordinate.Name, subordinate.Surname, subordinate.PassportId)
            => SetChief(subordinate.Chief);

        public override void SetChief(Employee chief)
        {
            ArgumentNullException.ThrowIfNull(chief);

            if (chief is not TeamLead)
                throw new ReportsException($"Too low role to set {chief} as chief");

            Chief = chief;
        }

        public override void AddSubordinate(Subordinate subordinate)
        {
            ArgumentNullException.ThrowIfNull(subordinate);

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