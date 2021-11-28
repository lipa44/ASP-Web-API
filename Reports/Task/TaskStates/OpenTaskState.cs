using Reports.Employees;
using Reports.Employees.Abstractions;

namespace Reports.Task.TaskStates
{
    public class OpenTaskState : TaskState
    {
        public override bool IsAbleToChangeName(Employee changer, string newTaskName)
            => changer is Supervisor or TeamLead;
        public override bool IsAbleToChangeContent(Employee changer, string newTaskContent)
            => changer is Supervisor or TeamLead;
        public override bool IsAbleToChangeTaskState(Employee changer, TaskState newTaskState)
            => newTaskState is not ResolvedTaskState;
        public override bool IsAbleToAddComment(Employee changer, string newComment) => true;
        public override bool IsAbleToAddImplementor(Employee changer, Employee newImplementor) => true;
    }
}