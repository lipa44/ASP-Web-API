#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using ReportsLibrary.Employees.Abstractions;
using ReportsLibrary.Tasks.TaskStates;

namespace ReportsLibrary.Tasks.TaskSnapshots
{
    public class TaskSnapshot : ITaskSnapshot
    {
        private readonly string _name;
        private readonly string? _content;
        private readonly List<TaskComment> _comments;
        private readonly List<TaskModification> _modifications;
        private readonly Employee _implementer;
        private readonly DateTime _modificationTime;
        private readonly TaskState _taskState;

        public TaskSnapshot(Task task, TaskState taskState)
        {
            _name = task.TaskName;
            _content = task.Content;
            _modificationTime = task.ModificationTime;
            _comments = task.Comments.ToList();
            _modifications = task.Modifications.ToList();
            _implementer = task.Implementer;
            _taskState = taskState;
        }

        public string GetName() => _name;
        public string? GetContent() => _content;
        public List<TaskComment> GetComments() => _comments;
        public List<TaskModification> GetModifications() => _modifications;

        public Employee GetImplementer() => _implementer;
        public DateTime GetModificationTime() => _modificationTime;
        public TaskState GetTaskState() => _taskState;
    }
}