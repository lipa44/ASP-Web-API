namespace Domain.Entities.Tasks.TaskOperationValidators.Abstractions;

public interface ITaskOperationValidator
{
    public bool HasPermissionToSetTitle(Employee changer);
    public bool HasPermissionToSetContent(Employee changer);
    public bool HasPermissionToAddComment(Employee changer);
    public bool HasPermissionToSetOwner(Employee changer);
    public bool HasPermissionToSetState(Employee changer);
    public bool HasPermissionToSetSprint(Employee changer);
}