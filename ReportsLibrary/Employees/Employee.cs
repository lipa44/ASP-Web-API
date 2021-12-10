using System;
using System.Collections.Generic;
using System.Linq;
using ReportsLibrary.Tools;

namespace ReportsLibrary.Employees
{
    public class Employee
    {
        public Employee() { }

        public Employee(string name, string surname, Guid id, EmployeeRoles role)
        {
            ReportsException.ThrowIfNullOrWhiteSpace(name);
            ReportsException.ThrowIfNullOrWhiteSpace(surname);

            if (id == Guid.Empty)
                throw new ReportsException("Passport ID to create employee is empty");

            Name = name;
            Surname = surname;
            Id = id;
            Role = role;
        }

        public string Name { get; init; }
        public string Surname { get; init; }
        public Guid Id { get; init; }
        public EmployeeRoles Role { get; init; }
        public IReadOnlyCollection<Employee> Subordinates => Employees;
        public Employee Chief { get; protected set; }
        protected List<Employee> Employees { get; init; } = new ();

        public void SetChief(Employee chief)
        {
            ArgumentNullException.ThrowIfNull(chief);

            Chief = chief;
        }

        public void AddSubordinate(Employee subordinate)
        {
            ArgumentNullException.ThrowIfNull(subordinate);

            if (IsSubordinateExist(subordinate))
                throw new ReportsException($"Employee {subordinate} already exists in {this}'s subordinates");

            subordinate.SetChief(this);
            Employees.Add(subordinate);
        }

        public void RemoveSubordinate(Employee subordinate)
        {
            ArgumentNullException.ThrowIfNull(subordinate);

            if (!Employees.Remove(subordinate))
                throw new ReportsException($"Employee {subordinate} doesn't exist in {this}'s subordinates");
        }

        public override string ToString() => $"{Name} {Surname}";

        public override bool Equals(object obj) => Equals(obj as Employee);
        public override int GetHashCode() => HashCode.Combine(Id, Name, Surname);

        protected bool IsSubordinateExist(Employee employee) => Employees.Any(e => e.Equals(employee));

        private bool Equals(Employee employee) => employee is not null && employee.Id == Id
                                                                        && employee.Name == Name
                                                                        && employee.Surname == Surname;
    }
}