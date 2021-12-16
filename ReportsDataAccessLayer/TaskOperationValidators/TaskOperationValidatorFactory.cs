using ReportsDataAccessLayer.TaskOperationValidators.Abstractions;
using ReportsLibrary.Tasks;
using ReportsLibrary.Tasks.TaskStates;
using ReportsLibrary.Tools;

namespace ReportsDataAccessLayer.TaskOperationValidators
{
    public class TaskOperationValidatorFactory : ITaskOperationValidatorFactory
    {
        public ITaskOperationValidator CreateValidator(ReportsTask reportsTask)
            => reportsTask.State switch
            {
                OpenTaskState => new OpenTaskOperationValidator(),
                ActiveTaskState => new ActiveTaskOperationValidator(),
                ResolvedTaskState => new ResolvedTaskOperationValidator(),
                _ => throw new ReportsException("Task state unrecognized")
            };
    }
}