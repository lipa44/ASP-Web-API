#nullable enable
using System;

namespace Reports.Employees
{
    public abstract class Employee
    {
        public Employee(string name, string surname)
        {
            Name = name;
            Surname = surname;
            Id = Guid.NewGuid();
        }

        protected string Name { get; }
        protected string Surname { get; }
        protected Guid Id { get; }

        public override bool Equals(object? obj) => Equals(obj as Employee);
        public override int GetHashCode() => HashCode.Combine(Id, Name, Surname);
        private bool Equals(Employee? employee) => employee is not null && employee.Id == Id;
    }
}