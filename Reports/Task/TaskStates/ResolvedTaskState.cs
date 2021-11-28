using Reports.Employees;
using Reports.Employees.Abstractions;

namespace Reports.Task.TaskStates
{
    public class ResolvedTaskState : TaskState
    {
        public override bool IsAbleToChangeTaskState(Employee changer, TaskState newTaskState)
            => changer is TeamLead;
        public override bool IsAbleToAddComment(Employee changer, string newComment) => true;
        public override bool IsAbleToChangeName(Employee changer, string newTaskName) => false;
        public override bool IsAbleToChangeContent(Employee changer, string newTaskContent) => false;
        public override bool IsAbleToAddImplementor(Employee changer, Employee newImplementor) => false;
    }
}