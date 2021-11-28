using Reports.Employees.Abstractions;
using Reports.Task.TaskStates;

namespace Reports.Task
{
    public interface ITask
    {
        public void ChangeName(Employee changer, string newTaskName);
        public void ChangeContent(Employee changer, string newTaskContent);
        public void AddComment(Employee changer, string comment);
        public void AddImplementor(Employee changer, Employee newImplementor);
        public void ChangeState(Employee changer, TaskState newTaskState);
        public void MakeSnapshot();
        public void RestorePreviousSnapshot();
        public void RestoreNextSnapshot();
    }
}