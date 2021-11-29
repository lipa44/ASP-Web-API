using System;
using ReportsLibrary.Employees.Abstractions;
using ReportsLibrary.Tools;

namespace ReportsLibrary.Tasks
{
    public class TaskModification
    {
        public TaskModification(Employee changer, object dataChanged, TaskChanges taskChange, DateTime modificationTime)
        {
            ArgumentNullException.ThrowIfNull(changer);
            ArgumentNullException.ThrowIfNull(dataChanged);

            if (modificationTime == default)
                throw new ArgumentNullException(nameof(modificationTime));

            Changer = changer;
            DataChanged = dataChanged;
            TaskChange = taskChange;
            ModificationTime = modificationTime;
        }

        public Employee Changer { get; }
        public object DataChanged { get; }
        public TaskChanges TaskChange { get; }
        public DateTime ModificationTime { get; }
        public Guid Id { get; } = Guid.NewGuid();
    }
}