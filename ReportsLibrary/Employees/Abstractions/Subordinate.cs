using System;
using System.Collections.Generic;
using System.Linq;

namespace ReportsLibrary.Employees.Abstractions
{
    public abstract class Subordinate : Employee
    {
        protected Subordinate(string name, string surname, Guid passportId)
            : base(name, surname, passportId)
        {
            Chief = this;
        }

        public IReadOnlyCollection<Employee> Subordinates => Employees;
        public Employee Chief { get; protected set; }
        protected List<Employee> Employees { get; } = new ();

        public abstract void SetChief(Employee chief);
        public abstract void AddSubordinate(Subordinate subordinate);
        public abstract void RemoveSubordinate(Subordinate subordinate);

        protected bool IsSubordinateExist(Employee employee) => Employees.Any(e => e.Equals(employee));
    }
}