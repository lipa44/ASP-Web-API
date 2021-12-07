using ReportsLibrary.Employees;

namespace ReportsLibrary.Tasks.TaskStates
{
    public interface ITaskState
    {
        public bool IsAbleToChangeContent(Employee changer, string newTaskContent);
        public bool IsAbleToAddComment(Employee changer, string newComment);
        public bool IsAbleToAddImplementor(Employee changer, Employee newImplementor);
        public bool IsAbleToChangeTaskState(Employee changer, ITaskState newTaskState);

        public string? ToString() => GetType().Name;
    }
}