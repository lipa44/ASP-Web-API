#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using Reports.Employees.Abstractions;
using Reports.Task.TaskStates;

namespace Reports.Task.TaskSnapshots
{
    public class TaskSnapshot : ITaskSnapshot
    {
        private readonly string _name;
        private readonly string? _content;
        private readonly List<TaskComment> _comments;
        private readonly List<Employee> _implementers;
        private readonly DateTime _modificationTime;
        private readonly TaskState _taskState;

        public TaskSnapshot(Task task, TaskState taskState)
        {
            _name = task.Name;
            _content = task.Content;
            _modificationTime = task.ModificationTime;
            _comments = task.Comments.ToList();
            _implementers = task.Implementers.ToList();
            _taskState = taskState;
        }

        public string GetName() => _name;
        public string? GetContent() => _content;
        public List<TaskComment> GetComments() => _comments;
        public List<Employee> GetImplementors() => _implementers;
        public DateTime GetModificationTime() => _modificationTime;
        public TaskState GetTaskState() => _taskState;
    }
}