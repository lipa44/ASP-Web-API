using ReportsLibrary.Tasks;

namespace Reports.TaskOperationValidators.Abstractions
{
    public interface ITaskOperationValidatorFactory
    {
        ITaskOperationValidator CreateValidator(Task task);
    }
}