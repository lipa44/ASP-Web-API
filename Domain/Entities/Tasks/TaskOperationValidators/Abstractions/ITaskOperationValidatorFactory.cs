namespace Domain.Entities.Tasks.TaskOperationValidators.Abstractions;

public interface ITaskOperationValidatorFactory
{
    ITaskOperationValidator CreateValidator(ReportsTask reportsTask);
}