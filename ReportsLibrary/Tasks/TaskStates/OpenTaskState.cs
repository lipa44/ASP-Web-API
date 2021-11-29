using ReportsLibrary.Employees;
using ReportsLibrary.Employees.Abstractions;

namespace ReportsLibrary.Tasks.TaskStates
{
    public class OpenTaskState : TaskState
    {
        public override bool IsAbleToChangeContent(Employee changer, string newTaskContent)
            => changer is Supervisor or TeamLead;
        public override bool IsAbleToChangeTaskState(Employee changer, TaskState newTaskState)
            => newTaskState is not ResolvedTaskState && newTaskState != this;
        public override bool IsAbleToAddComment(Employee changer, string newComment) => true;
        public override bool IsAbleToAddImplementor(Employee changer, Employee newImplementor) => true;
    }
}