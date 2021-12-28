namespace ReportsDomain.Tasks.TaskStates;

using Employees;
using Enums;

public class ActiveTaskState : TaskState
{
    public override bool IsAbleToChangeContent(Employee changer, string newTaskContent)
        => changer.Role is EmployeeRoles.Supervisor or EmployeeRoles.TeamLead;

    public override bool IsAbleToChangeTaskState(Employee changer, TaskState newTaskState)
        => changer.Role is EmployeeRoles.Supervisor or EmployeeRoles.TeamLead && newTaskState != this;

    public override bool IsAbleToAddComment(Employee changer, string newComment) => true;
    public override bool IsAbleToAddImplementor(Employee changer, Employee newImplementor) => true;
}