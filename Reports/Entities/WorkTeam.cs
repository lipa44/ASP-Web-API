using System.Collections.Generic;
using System.Linq;
using Reports.Employees;
using Reports.Tools;

namespace Reports.Entities
{
    public class WorkTeam
    {
        private readonly List<Task.Task> _tasks;
        private readonly Employee _teamLead;
        private readonly List<Supervisor> _supervisors;
        private readonly List<OrdinaryEmployee> _employees;

        public WorkTeam(Employee teamLead)
        {
            _tasks = new List<Task.Task>();
            _teamLead = teamLead;
            _supervisors = new List<Supervisor>();
            _employees = new List<OrdinaryEmployee>();
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