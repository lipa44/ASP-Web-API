namespace ReportsDomain.Tasks.TaskOperationValidators;

using Entities;
using Enums;
using Abstractions;

public class ResolvedTaskOperationValidator : ITaskOperationValidator
{
    public bool HasPermissionToSetTitle(Employee changer) => changer.Role is EmployeeRoles.TeamLead;
    public bool HasPermissionToSetContent(Employee changer) => changer.Role is EmployeeRoles.TeamLead;
    public bool HasPermissionToAddComment(Employee changer) => changer.Role is EmployeeRoles.TeamLead;
    public bool HasPermissionToSetOwner(Employee changer) => changer.Role is EmployeeRoles.TeamLead;
    public bool HasPermissionToSetState(Employee changer) => changer.Role is EmployeeRoles.TeamLead;
    public bool HasPermissionToSetSprint(Employee changer) => changer.Role is EmployeeRoles.TeamLead;
}