using System;
using System.Collections.Generic;
using ReportsLibrary.Employees;
using ReportsLibrary.Tasks.TaskStates;

namespace ReportsLibrary.Tasks.TaskSnapshots
{
    public interface ITaskSnapshot
    {
        public string Name { get; init; }
        public string Content { get; init; }
        public List<TaskComment> Comments { get; init; }
        public List<TaskModification> Modifications { get; init; }
        public Employee Owner { get; init; }
        public DateTime ModificationTime { get; init; }
        public Tools.TaskStates TaskState { get; init; }
    }
}