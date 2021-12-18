using ReportsLibrary.Employees;

namespace ReportsLibrary.TaskOperationValidators.Abstractions
{
    public interface ITaskOperationValidator
    {
        public bool HasPermissionToSetContent(Employee changer);
        public bool HasPermissionToAddComment(Employee changer);
        public bool HasPermissionToSetImplementer(Employee changer);
        public bool HasPermissionToSetState(Employee changer);
    }
}