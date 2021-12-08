using System;
using System.Collections.Generic;
using System.Linq;
using ReportsLibrary.Employees;
using ReportsLibrary.Tasks.TaskStates;

namespace ReportsLibrary.Tasks.TaskSnapshots
{
    public class TaskSnapshot : ITaskSnapshot
    {
        public TaskSnapshot() { }

        public string Name { get; init; }
        public string Content { get; init; }
        public List<TaskComment> Comments { get; init; }
        public List<TaskModification> Modifications { get; init; }
        public Employee Implementer { get; init; }
        public DateTime ModificationTime { get; init; }
        public ITaskState TaskState { get; init; }
    }
}