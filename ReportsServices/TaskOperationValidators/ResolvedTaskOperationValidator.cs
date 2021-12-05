using Reports.TaskOperationValidators.Abstractions;
using ReportsLibrary.Employees;

namespace Reports.TaskOperationValidators
{
    public class ResolvedTaskOperationValidator : ITaskOperationValidator
    {
        public bool HasPermissionToContent(Employee changer) => false;
        public bool HasPermissionToAddComment(Employee changer) => true;
        public bool HasPermissionToChangeImplementer(Employee changer) => false;
        public bool HasPermissionToChangeState(Employee changer) => changer is TeamLead;
    }
}