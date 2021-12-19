using ReportsLibrary.Tasks;

namespace ReportsLibrary.TaskOperationValidators.Abstractions
{
    public interface ITaskOperationValidatorFactory
    {
        ITaskOperationValidator CreateValidator(ReportsTask reportsTask);
    }
}