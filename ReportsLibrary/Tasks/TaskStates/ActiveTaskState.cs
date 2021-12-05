using ReportsLibrary.Employees;

namespace ReportsLibrary.Tasks.TaskStates
{
    public class ActiveTaskState : TaskState
    {
        public override bool IsAbleToChangeContent(Employee changer, string newTaskContent)
            => changer is Supervisor or TeamLead;
        public override bool IsAbleToChangeTaskState(Employee changer, TaskState newTaskState)
            => changer is Supervisor or TeamLead && newTaskState != this;
        public override bool IsAbleToAddComment(Employee changer, string newComment) => true;
        public override bool IsAbleToAddImplementor(Employee changer, Employee newImplementor) => true;
    }
}