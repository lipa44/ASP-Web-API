#nullable enable
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using ReportsLibrary.Tasks;
using ReportsLibrary.Tools;

namespace ReportsLibrary.Employees
{
    public abstract class Employee
    {
        protected Employee(string name, string surname, Guid id)
        {
            ReportsException.ThrowIfNullOrWhiteSpace(name);
            ReportsException.ThrowIfNullOrWhiteSpace(surname);

            if (id == Guid.Empty)
                throw new ReportsException("Passport ID to create employee is empty");

            Name = name;
            Surname = surname;
            Id = id;
        }

        public string Name { get; }
        public string Surname { get; }
        public Guid Id { get; }
        public IReadOnlyCollection<Employee> Subordinates => Employees;
        public Employee? Chief { get; protected set; }
        protected List<Employee> Employees { get; } = new ();

        public abstract void SetChief(Employee chief);
        public abstract void AddSubordinate(Employee subordinate);
        public abstract void RemoveSubordinate(Employee subordinate);

        public override string ToString() => $"{Name} {Surname}";

        /// <summary>
        /// Returns if employee has higher role than this entity's.
        /// TeamLead is the highest role in this system. So it doesn't need any role checks and always return false.
        /// </summary>
        /// <param name="employee">Employee to compare if his role is higher than this entity's.</param>
        /// <returns>If employee role is higher than this entity's.</returns>
        public abstract bool IsLowerOrEqualRole(Employee employee);

        public override bool Equals(object? obj) => Equals(obj as Employee);
        public override int GetHashCode() => HashCode.Combine(Id, Name, Surname);

        protected bool IsSubordinateExist(Employee employee) => Employees.Any(e => e.Equals(employee));

        private bool Equals(Employee? employee) => employee is not null && employee.Id == Id
                                                                        && employee.Name == Name
                                                                        && employee.Surname == Surname;
    }
}