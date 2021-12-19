using ReportsLibrary.Employees;
using ReportsLibrary.Enums;
using ReportsLibrary.TaskOperationValidators.Abstractions;

namespace ReportsLibrary.TaskOperationValidators;

public class ActiveTaskOperationValidator : ITaskOperationValidator
{
    public bool HasPermissionToSetContent(Employee changer) =>
        changer.Role is EmployeeRoles.Supervisor or EmployeeRoles.TeamLead;

    public bool HasPermissionToSetState(Employee changer) =>
        changer.Role is EmployeeRoles.Supervisor or EmployeeRoles.TeamLead;

    public bool HasPermissionToAddComment(Employee changer) => true;
    public bool HasPermissionToSetImplementer(Employee changer) => true;
}