using System.Collections.Generic;
using System.Linq;
using Reports.Tools;

namespace Reports.Entities
{
    public class Sprint
    {
        private readonly List<Task.Task> _tasks;

        public Sprint()
        {
            _tasks = new List<Task.Task>();
        }

        public IReadOnlyCollection<Task.Task> Tasks => _tasks;

        public void AddTask(Task.Task task)
        {
            if (task is null)
                throw new ReportsException("Task to add into sprint is null");

            if (IsTaskExist(task))
                throw new ReportsException("Task to add into sprint already exists");

            _tasks.Add(task);
        }

        public void RemoveTask(Task.Task task)
        {
            if (task is null)
                throw new ReportsException("Task to remove from sprint is null");

            if (!IsTaskExist(task))
                throw new ReportsException("Task to remove from sprint doesn't exist");

            _tasks.Remove(task);
        }

        private bool IsTaskExist(Task.Task task) => _tasks.Any(t => t.Equals(task));
    }
}