namespace ReportsDomain.Tasks.TaskOperationValidators;

using Employees;
using Enums;
using Abstractions;

public class OpenTaskOperationValidator : ITaskOperationValidator
{
    public bool HasPermissionToSetTitle(Employee changer)
        => changer.Role is EmployeeRoles.Supervisor or EmployeeRoles.TeamLead;
    public bool HasPermissionToSetContent(Employee changer)
        => changer.Role is EmployeeRoles.Supervisor or EmployeeRoles.TeamLead;
    public bool HasPermissionToSetState(Employee changer) => true;
    public bool HasPermissionToAddComment(Employee changer) => true;
    public bool HasPermissionToSetOwner(Employee changer) => true;
}