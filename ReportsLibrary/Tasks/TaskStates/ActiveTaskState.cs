using ReportsLibrary.Employees;

namespace ReportsLibrary.Tasks.TaskStates
{
    public class ActiveTaskState : ITaskState
    {
        public bool IsAbleToChangeContent(Employee changer, string newTaskContent)
            => changer is Supervisor or TeamLead;
        public bool IsAbleToChangeTaskState(Employee changer, ITaskState newTaskState)
            => changer is Supervisor or TeamLead && newTaskState != this;
        public bool IsAbleToAddComment(Employee changer, string newComment) => true;
        public bool IsAbleToAddImplementor(Employee changer, Employee newImplementor) => true;
    }
}