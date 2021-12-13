using System;
using ReportsLibrary.Employees;
using ReportsLibrary.Tools;

namespace ReportsLibrary.Tasks
{
    public class TaskModification
    {
        public TaskModification() { }

        public TaskModification(Employee changer, object data, TaskModificationActions action, DateTime modificationTime)
        {
            // ArgumentNullException.ThrowIfNull(changer);
            // ArgumentNullException.ThrowIfNull(data);
            if (modificationTime == default)
                throw new ReportsException("Task change time can't be default");

            ChangerId = changer.Id;
            Data = data.ToString();
            Action = action;
            ModificationTime = modificationTime;
        }

        public Guid? ChangerId { get; init; }
        public string Data { get; init; }
        public TaskModificationActions Action { get; init; }
        public DateTime ModificationTime { get; init; }
        public Guid Id { get; init; } = Guid.NewGuid();
    }
}