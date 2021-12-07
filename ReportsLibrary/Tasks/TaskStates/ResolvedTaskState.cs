using ReportsLibrary.Employees;

namespace ReportsLibrary.Tasks.TaskStates
{
    public class ResolvedTaskState : ITaskState
    {
        public bool IsAbleToChangeTaskState(Employee changer, ITaskState newTaskState)
            => changer is TeamLead && newTaskState != this;
        public bool IsAbleToAddComment(Employee changer, string newComment) => true;
        public bool IsAbleToChangeContent(Employee changer, string newTaskContent) => false;
        public bool IsAbleToAddImplementor(Employee changer, Employee newImplementor) => false;
    }
}