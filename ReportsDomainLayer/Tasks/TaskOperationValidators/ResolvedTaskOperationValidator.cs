using ReportsLibrary.Employees;
using ReportsLibrary.Enums;
using ReportsLibrary.TaskOperationValidators.Abstractions;

namespace ReportsLibrary.TaskOperationValidators;

public class ResolvedTaskOperationValidator : ITaskOperationValidator
{
    public bool HasPermissionToSetContent(Employee changer) => false;
    public bool HasPermissionToAddComment(Employee changer) => true;
    public bool HasPermissionToSetImplementer(Employee changer) => false;
    public bool HasPermissionToSetState(Employee changer) => changer.Role is EmployeeRoles.TeamLead;
}