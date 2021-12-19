using ReportsLibrary.Enums;
using ReportsLibrary.TaskOperationValidators.Abstractions;
using ReportsLibrary.Tasks;
using ReportsLibrary.Tools;

namespace ReportsLibrary.TaskOperationValidators;

public class TaskOperationValidatorFactory : ITaskOperationValidatorFactory
{
    public ITaskOperationValidator CreateValidator(ReportsTask reportsTask)
        => reportsTask.State switch
        {
            TaskStates.Open => new OpenTaskOperationValidator(),
            TaskStates.Active => new ActiveTaskOperationValidator(),
            TaskStates.Resolved => new ResolvedTaskOperationValidator(),
            _ => throw new ReportsException("Task state unrecognized")
        };
}