#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using ReportsLibrary.Tools;
using ReportsTask = ReportsLibrary.Tasks.Task;

namespace ReportsLibrary.Entities
{
    public class Sprint
    {
        private readonly List<ReportsTask> _tasks = new ();

        public Sprint(DateTime expirationDate)
        {
            if (expirationDate == default)
                throw new ReportsException("Sprint's expiration date must be not default");

            ExpirationDate = expirationDate;
        }

        public DateTime ExpirationDate { get; }
        public Guid Id { get; } = Guid.NewGuid();
        public IReadOnlyCollection<ReportsTask> Tasks => _tasks;

        public void AddTask(ReportsTask task)
        {
            ArgumentNullException.ThrowIfNull(task);

            if (IsTaskExist(task))
                throw new ReportsException("Task to add into sprint already exists");

            _tasks.Add(task);
        }

        public void RemoveTask(ReportsTask task)
        {
            ArgumentNullException.ThrowIfNull(task);

            if (!_tasks.Remove(task))
                throw new ReportsException("Task to remove from sprint doesn't exist");
        }

        public override bool Equals(object? obj) => Equals(obj as Sprint);
        public override int GetHashCode() => HashCode.Combine(Id);
        private bool Equals(Sprint? sprint) => sprint is not null && sprint.Id == Id;

        private bool IsTaskExist(ReportsTask task) => _tasks.Any(t => t.Equals(task));
    }
}