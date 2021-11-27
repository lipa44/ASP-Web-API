#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;

namespace Reports.Task.TaskSnapshot
{
    public class TaskSnapshot : ITaskSnapshot
    {
        private readonly string _name;
        private readonly string? _content;
        private readonly List<TaskComment> _comments;
        private readonly DateTime _modificationTime;
        private readonly TaskState _taskState;

        public TaskSnapshot(Task task, TaskState taskState)
        {
            _name = task.Name;
            _content = task.Content;
            _modificationTime = task.ModificationTime;
            _comments = task.Comments.ToList();
            _taskState = taskState;
        }

        public string GetName() => _name;
        public string? GetContent() => _content;
        public List<TaskComment> GetComments() => _comments;
        public DateTime GetModificationTime() => _modificationTime;
        public TaskState GetTaskState() => _taskState;
    }
}