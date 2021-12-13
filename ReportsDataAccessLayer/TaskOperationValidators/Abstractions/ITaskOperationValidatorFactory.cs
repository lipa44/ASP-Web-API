using Task = ReportsLibrary.Tasks.Task;

namespace ReportsDataAccessLayer.TaskOperationValidators.Abstractions
{
    public interface ITaskOperationValidatorFactory
    {
        ITaskOperationValidator CreateValidator(Task task);
    }
}