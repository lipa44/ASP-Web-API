using ReportsLibrary.Employees;
using ReportsLibrary.Enums;
using ReportsLibrary.Tasks.TaskOperationValidators.Abstractions;

namespace ReportsLibrary.Tasks.TaskOperationValidators;

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