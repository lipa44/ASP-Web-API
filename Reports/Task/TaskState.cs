using Reports.Employees;

namespace Reports.Task
{
    public abstract class TaskState
    {
        protected Task Task { get; set; }

        public void SetTask(Task task)
        {
            Task = task;
        }

        public abstract void AddComment(Employee employee, string comment);
    }
}