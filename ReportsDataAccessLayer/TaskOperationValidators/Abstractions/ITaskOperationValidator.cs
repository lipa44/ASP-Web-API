using ReportsLibrary.Employees;

namespace ReportsDataAccessLayer.TaskOperationValidators.Abstractions
{
    public interface ITaskOperationValidator
    {
        public bool HasPermissionToContent(Employee changer);
        public bool HasPermissionToAddComment(Employee changer);
        public bool HasPermissionToChangeImplementer(Employee changer);
        public bool HasPermissionToChangeState(Employee changer);
    }
}