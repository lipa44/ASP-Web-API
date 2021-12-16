using System;
using System.Collections.Generic;
using System.Linq;
using ReportsLibrary.Entities;
using ReportsLibrary.Tasks;
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
        public Employee Chief { get; protected set; }
        public Guid? ChiefId { get; protected set; }

        public WorkTeam WorkTeam { get; protected set; }
        public Guid? WorkTeamId { get; protected set; }

        // public List<Employee> Subordinates { get; init; } = new ();
        public virtual ICollection<ReportsTask> Tasks { get; init; } = new List<ReportsTask>();
        public void SetChief(Employee chief)
        {
            ArgumentNullException.ThrowIfNull(chief);

            Chief = chief;
            ChiefId = chief.Id;
        }

        // public void AddSubordinate(Employee subordinate)
        // {
        //     ArgumentNullException.ThrowIfNull(subordinate);
        //
        //     if (IsSubordinateExist(subordinate))
        //         throw new ReportsException($"Employee {subordinate} already exists in {this}'s subordinates");
        //
        //     Subordinates.Add(subordinate);
        // }
        //
        // public void RemoveSubordinate(Employee subordinate)
        // {
        //     ArgumentNullException.ThrowIfNull(subordinate);
        //
        //     if (!Subordinates.Remove(subordinate))
        //         throw new ReportsException($"Employee {subordinate} doesn't exist in {this}'s subordinates");
        // }
        public void AddTask(ReportsTask reportsTask)
        {
            ArgumentNullException.ThrowIfNull(reportsTask);

            if (IsTaskExist(reportsTask))
                throw new ReportsException($"Task {reportsTask} already exists in {this}'s tasks");

            Tasks.Add(reportsTask);
        }

        public void RemoveTask(ReportsTask reportsTask)
        {
            ArgumentNullException.ThrowIfNull(reportsTask);

            if (!Tasks.Remove(reportsTask))
                throw new ReportsException($"Task {reportsTask} doesn't exist in {this}'s tasks");
        }

        public override string ToString() => $"{Name} {Surname}";

        public override bool Equals(object obj) => Equals(obj as Employee);
        public override int GetHashCode() => HashCode.Combine(Id, Name, Surname);

        // protected bool IsSubordinateExist(Employee employee) => Subordinates.Any(e => e.Equals(employee));
        protected bool IsTaskExist(ReportsTask reportsTask) => Tasks.Any(t => t.Equals(reportsTask));
        private bool Equals(Employee employee) => employee is not null && employee.Id == Id
                                                                       && employee.Name == Name
                                                                       && employee.Surname == Surname;
    }
}