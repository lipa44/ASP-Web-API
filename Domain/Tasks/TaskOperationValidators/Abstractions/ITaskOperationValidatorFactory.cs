namespace Domain.Tasks.TaskOperationValidators.Abstractions;

public interface ITaskOperationValidatorFactory
{
    ITaskOperationValidator CreateValidator(ReportsTask reportsTask);
}