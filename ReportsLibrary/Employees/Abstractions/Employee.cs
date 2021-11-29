#nullable enable
using System;
using ReportsLibrary.Tools;

namespace ReportsLibrary.Employees.Abstractions
{
    public abstract class Employee
    {
        protected Employee(string name, string surname, Guid passportId)
        {
            ReportsException.ThrowIfNullOrWhiteSpace(name);
            ReportsException.ThrowIfNullOrWhiteSpace(surname);

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

        /// <summary>
        /// Returns if employee has higher role than this entity's.
        /// TeamLead is the highest role in this system. So it doesn't need any role checks and always return false.
        /// </summary>
        /// <param name="employee">Employee to compare if his role is higher than this entity's.</param>
        /// <returns>If employee role is higher than this entity's.</returns>
        public abstract bool IsHigherRole(Employee employee);

        public override bool Equals(object? obj) => Equals(obj as Employee);
        public override int GetHashCode() => HashCode.Combine(PassportId, Name, Surname);

        private bool Equals(Employee? employee) => employee is not null && employee.PassportId == PassportId
                                                                        && employee.Name == Name
                                                                        && employee.Surname == Surname
                                                                        && employee.GetType() == GetType();
    }
}