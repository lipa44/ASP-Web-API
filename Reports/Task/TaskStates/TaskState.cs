using Reports.Employees.Abstractions;
using Reports.Tools;

namespace Reports.Task.TaskStates
{
    public abstract class TaskState
    {
        protected Task Task { get; set; }

        public TaskState SetTask(Task task)
        {
            Task = task ?? throw new ReportsException("Task to set in task state is null");
            return this;
        }

        public TaskState Clone() => MemberwiseClone() as TaskState;

        public abstract bool IsAbleToChangeName(Employee changer, string newTaskName);
        public abstract bool IsAbleToChangeContent(Employee changer, string newTaskContent);
        public abstract bool IsAbleToAddComment(Employee changer, string newComment);
        public abstract bool IsAbleToAddImplementor(Employee changer, Employee newImplementor);
        public abstract bool IsAbleToChangeTaskState(Employee changer, TaskState newTaskState);

        public override string ToString() => GetType().Name;
    }
}