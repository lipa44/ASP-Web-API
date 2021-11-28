#nullable enable
using System;
using Reports.Tools;

namespace Reports.Employees.Abstractions
{
    public abstract class Employee
    {
        protected Employee(string name, string surname, Guid passportId)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ReportsException("Name to create employee is empty");

            if (string.IsNullOrWhiteSpace(surname))
                throw new ReportsException("Surname to create employee is empty");

            if (passportId == Guid.Empty)
                throw new ReportsException("Passport ID to create employee is empty");

            Name = name;
            Surname = surname;
            PassportId = passportId;
        }

        public string Name { get; }
        public string Surname { get; }
        public Guid PassportId { get; }

        public override string ToString() => $"{Name} {Surname}";

        public override bool Equals(object? obj) => Equals(obj as Employee);
        public override int GetHashCode() => HashCode.Combine(PassportId, Name, Surname);

        private bool Equals(Employee? employee) => employee is not null && employee.PassportId == PassportId
                                                                        && employee.Name == Name
                                                                        && employee.Surname == Surname;
    }
}