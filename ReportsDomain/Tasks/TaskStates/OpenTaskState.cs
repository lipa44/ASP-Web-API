using ReportsDomain.Employees;
using ReportsDomain.Enums;

namespace ReportsDomain.Tasks.TaskStates;

public class OpenTaskState : TaskState
{
    public override bool IsAbleToChangeContent(Employee changer, string newTaskContent)
        => changer.Role is EmployeeRoles.Supervisor or EmployeeRoles.TeamLead;
    public override bool IsAbleToChangeTaskState(Employee changer, TaskState newTaskState)
        => newTaskState is not ResolvedTaskState && newTaskState != this;
    public override bool IsAbleToAddComment(Employee changer, string newComment) => true;
    public override bool IsAbleToAddImplementor(Employee changer, Employee newImplementor) => true;
}