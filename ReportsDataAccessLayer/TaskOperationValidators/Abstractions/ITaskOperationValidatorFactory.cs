using ReportsLibrary.Tasks;

namespace ReportsDataAccessLayer.TaskOperationValidators.Abstractions
{
    public interface ITaskOperationValidatorFactory
    {
        ITaskOperationValidator CreateValidator(ReportsTask reportsTask);
    }
}