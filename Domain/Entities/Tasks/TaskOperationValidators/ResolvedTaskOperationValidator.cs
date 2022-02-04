using Domain.Entities.Tasks.TaskOperationValidators.Abstractions;
using Domain.Enums;

namespace Domain.Entities.Tasks.TaskOperationValidators;

public class ResolvedTaskOperationValidator : ITaskOperationValidator
{
    public bool HasPermissionToSetTitle(Employee changer) => changer.Role is EmployeeRoles.TeamLead;
    public bool HasPermissionToSetContent(Employee changer) => changer.Role is EmployeeRoles.TeamLead;
    public bool HasPermissionToAddComment(Employee changer) => changer.Role is EmployeeRoles.TeamLead;
    public bool HasPermissionToSetOwner(Employee changer) => changer.Role is EmployeeRoles.TeamLead;
    public bool HasPermissionToSetState(Employee changer) => changer.Role is EmployeeRoles.TeamLead;
    public bool HasPermissionToSetSprint(Employee changer) => changer.Role is EmployeeRoles.TeamLead;
}