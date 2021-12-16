using ReportsLibrary.Employees;

namespace ReportsLibrary.Tasks
{
    public interface ITask
    {
        public void ChangeContent(Employee changer, string newContent);
        public void AddComment(Employee changer, string comment);
        public void SetOwner(Employee changer, Employee newImplementer);
        public void SetState(Employee changer, Tools.TaskStates newState);

        // public void MakeSnapshot();
        // public void RestorePreviousSnapshot();
        // public void RestoreNextSnapshot();
    }
}