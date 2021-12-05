using Reports.TaskOperationValidators.Abstractions;
using ReportsLibrary.Employees;

namespace Reports.TaskOperationValidators
{
    public class ActiveTaskOperationValidator : ITaskOperationValidator
    {
        public bool HasPermissionToContent(Employee changer) => changer is Supervisor or TeamLead;
        public bool HasPermissionToChangeState(Employee changer) => changer is Supervisor or TeamLead;
        public bool HasPermissionToAddComment(Employee changer) => true;
        public bool HasPermissionToChangeImplementer(Employee changer) => true;
    }
}