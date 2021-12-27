namespace ReportsDomain.Tasks.TaskOperationValidators;

using ReportsDomain.Employees;
using ReportsDomain.Enums;
using ReportsDomain.Tasks.TaskOperationValidators.Abstractions;

public class ResolvedTaskOperationValidator : ITaskOperationValidator
{
    public bool HasPermissionToSetTitle(Employee changer) => changer.Role is EmployeeRoles.TeamLead;
    public bool HasPermissionToSetContent(Employee changer) => changer.Role is EmployeeRoles.TeamLead;
    public bool HasPermissionToAddComment(Employee changer) => changer.Role is EmployeeRoles.TeamLead;
    public bool HasPermissionToSetOwner(Employee changer) => changer.Role is EmployeeRoles.TeamLead;
    public bool HasPermissionToSetState(Employee changer) => changer.Role is EmployeeRoles.TeamLead;
}