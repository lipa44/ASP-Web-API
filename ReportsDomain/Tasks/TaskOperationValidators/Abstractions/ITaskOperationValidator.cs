using ReportsDomain.Employees;

namespace ReportsDomain.Tasks.TaskOperationValidators.Abstractions
{
    public interface ITaskOperationValidator
    {
        public bool HasPermissionToSetTitle(Employee changer);
        public bool HasPermissionToSetContent(Employee changer);
        public bool HasPermissionToAddComment(Employee changer);
        public bool HasPermissionToSetOwner(Employee changer);
        public bool HasPermissionToSetState(Employee changer);
    }
}