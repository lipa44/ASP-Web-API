using Reports.TaskOperationValidators.Abstractions;
using ReportsLibrary.Tasks;
using ReportsLibrary.Tasks.TaskStates;
using ReportsLibrary.Tools;

namespace Reports.TaskOperationValidators
{
    public class TaskOperationValidatorFactory : ITaskOperationValidatorFactory
    {
        public ITaskOperationValidator CreateValidator(Task task)
            => task.TaskState switch
            {
                OpenTaskState => new OpenTaskOperationValidator(),
                ActiveTaskState => new ActiveTaskOperationValidator(),
                ResolvedTaskState => new ResolvedTaskOperationValidator(),
                _ => throw new ReportsException("Task state unrecognized")
            };
    }
}