using System;
using ReportsLibrary.Employees;
using ReportsLibrary.Tools;

namespace ReportsLibrary.Tasks
{
    public class TaskModification
    {
        public TaskModification() { }

        public TaskModification(Employee changer, object data, TaskChangeActions taskChangeAction, DateTime modificationTime)
        {
            ArgumentNullException.ThrowIfNull(changer);
            ArgumentNullException.ThrowIfNull(data);

            if (modificationTime == default)
                throw new ReportsException("Task change time can't be default");

            Changer = changer;
            Data = data;
            TaskChangeAction = taskChangeAction;
            ModificationTime = modificationTime;
        }

        public Employee Changer { get; init; }
        public object Data { get; init; }
        public TaskChangeActions TaskChangeAction { get; init; }
        public DateTime ModificationTime { get; init; }
        public Guid Id { get; } = Guid.NewGuid();
    }
}