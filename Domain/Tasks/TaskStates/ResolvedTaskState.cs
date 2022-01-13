using Domain.Entities;
using Domain.Enums;

namespace Domain.Tasks.TaskStates;

public class ResolvedTaskState : TaskState
{
    public override bool IsAbleToChangeTaskState(Employee changer, TaskState newTaskState)
        => changer.Role is EmployeeRoles.TeamLead && newTaskState != this;
    public override bool IsAbleToAddComment(Employee changer, string newComment) => true;
    public override bool IsAbleToChangeContent(Employee changer, string newTaskContent) => false;
    public override bool IsAbleToAddImplementor(Employee changer, Employee newImplementor) => false;
}