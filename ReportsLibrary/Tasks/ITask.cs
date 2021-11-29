using ReportsLibrary.Employees.Abstractions;
using ReportsLibrary.Tasks.TaskStates;

namespace ReportsLibrary.Tasks
{
    public interface ITask
    {
        public void ChangeContent(Employee changer, string newTaskContent);
        public void AddComment(Employee changer, string comment);
        public void ChangeImplementer(Employee changer, Employee newImplementer);
        public void ChangeState(Employee changer, TaskState newTaskState);
        public void MakeSnapshot();
        public void RestorePreviousSnapshot();
        public void RestoreNextSnapshot();
    }
}