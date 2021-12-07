using ReportsLibrary.Employees;
using ReportsLibrary.Tasks.TaskStates;

namespace ReportsLibrary.Tasks
{
    public interface ITask
    {
        public void ChangeContent(Employee changer, string newContent);
        public void AddComment(Employee changer, string comment);
        public void ChangeImplementer(Employee changer, Employee newImplementer);
        public void ChangeState(Employee changer, ITaskState newState);
        public void MakeSnapshot();
        public void RestorePreviousSnapshot();
        public void RestoreNextSnapshot();
    }
}