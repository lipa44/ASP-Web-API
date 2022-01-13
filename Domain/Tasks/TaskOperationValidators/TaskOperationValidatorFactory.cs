using Domain.Tasks.TaskOperationValidators.Abstractions;
using Domain.Tools;

namespace Domain.Tasks.TaskOperationValidators;

public class TaskOperationValidatorFactory : ITaskOperationValidatorFactory
{
    public ITaskOperationValidator CreateValidator(ReportsTask reportsTask)
        => reportsTask.State switch
        {
            Enums.ReportTaskStates.Open => new OpenTaskOperationValidator(),
            Enums.ReportTaskStates.Active => new ActiveTaskOperationValidator(),
            Enums.ReportTaskStates.Resolved => new ResolvedTaskOperationValidator(),
            _ => throw new ReportsException("Task state unrecognized")
        };
}