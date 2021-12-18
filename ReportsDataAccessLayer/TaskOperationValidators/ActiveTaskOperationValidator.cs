using ReportsDataAccessLayer.TaskOperationValidators.Abstractions;
using ReportsLibrary.Employees;
using ReportsLibrary.Enums;

namespace ReportsDataAccessLayer.TaskOperationValidators;

public class ActiveTaskOperationValidator : ITaskOperationValidator
{
    public bool HasPermissionToContent(Employee changer) =>
        changer.Role is EmployeeRoles.Supervisor or EmployeeRoles.TeamLead;

    public bool HasPermissionToChangeState(Employee changer) =>
        changer.Role is EmployeeRoles.Supervisor or EmployeeRoles.TeamLead;

    public bool HasPermissionToAddComment(Employee changer) => true;
    public bool HasPermissionToChangeImplementer(Employee changer) => true;
}