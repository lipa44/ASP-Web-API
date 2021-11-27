#nullable enable
using System;
using System.Collections.Generic;
using Reports.Employees;
using Reports.Task.TaskSnapshot;
using Reports.Tools;

namespace Reports.Task
{
    public class Task
    {
        private readonly LinkedList<TaskSnapshot.TaskSnapshot> _snapshots;
        private List<TaskComment> _comments;
        private TaskState _taskState;

        public Task(string name)
        {
            Name = name;
            CreationTime = DateTime.Now;
            ModificationTime = DateTime.Now;
            Id = Guid.NewGuid();

            _snapshots = new LinkedList<TaskSnapshot.TaskSnapshot>();
            _comments = new List<TaskComment>();
            _taskState = new OpenTaskState();
            _taskState.SetTask(this);
        }

        public string Name { get; private set; }
        public string? Content { get;  private set; }
        public DateTime CreationTime { get; }
        public DateTime ModificationTime { get; private set; }
        public Guid Id { get; }
        public IReadOnlyCollection<TaskComment> Comments => _comments;

        public void ChangeName(Employee employee, string newTaskName)
        {
            if (!IsAbleToAddChanges())
                throw new ReportsException("Task state is resolved and you can't add changes");

            if (string.IsNullOrWhiteSpace(newTaskName))
                throw new ReportsException($"New task name to set in task {Name} is empty");

            MakeSnapshot();
            Name = newTaskName;
        }

        public void ChangeContent(Employee employee, string newTaskContent)
        {
            if (!IsAbleToAddChanges())
                throw new ReportsException("Task state is resolved and you can't add changes");

            if (string.IsNullOrWhiteSpace(newTaskContent))
                throw new ReportsException($"New task content to set in task {Name} is empty");

            MakeSnapshot();
            Content = newTaskContent;
        }

        public void AddComment(Employee employee, string comment)
        {
            if (!IsAbleToAddChanges())
                throw new ReportsException("Task state is resolved and you can't add changes");

            if (string.IsNullOrWhiteSpace(comment))
                throw new ReportsException($"Comment to set in task {Name} is empty");

            MakeSnapshot();
            _taskState.AddComment(employee, comment);
        }

        public void StartTask(Employee employee)
        {
            _taskState = new ActiveTaskState();
            _taskState.SetTask(this);
        }

        public void ResolveTask(Employee employee)
        {
            _taskState = new ResolvedTaskState();
            _taskState.SetTask(this);
        }

        public ITaskSnapshot MakeSnapshot() => new TaskSnapshot.TaskSnapshot(this, _taskState);

        public void RestoreSnapshot(ITaskSnapshot snapshot)
        {
            Name = snapshot.GetName();
            Content = snapshot.GetContent();
            ModificationTime = snapshot.GetModificationTime();
            _comments = snapshot.GetComments();
            _taskState = snapshot.GetTaskState();
        }

        public override bool Equals(object? obj) => Equals(obj as Task);
        public override int GetHashCode() => HashCode.Combine(Id);
        private bool Equals(Task? task) => task is not null && task.Id == Id;
        private bool IsAbleToAddChanges() => _taskState is not ResolvedTaskState;
    }
}