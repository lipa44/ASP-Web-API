using ReportsDataAccessLayer.TaskOperationValidators.Abstractions;
using ReportsLibrary.Employees;
using ReportsLibrary.Enums;

namespace ReportsDataAccessLayer.TaskOperationValidators;

public class ResolvedTaskOperationValidator : ITaskOperationValidator
{
    public bool HasPermissionToContent(Employee changer) => false;
    public bool HasPermissionToAddComment(Employee changer) => true;
    public bool HasPermissionToChangeImplementer(Employee changer) => false;
    public bool HasPermissionToChangeState(Employee changer) => changer.Role is EmployeeRoles.TeamLead;
}