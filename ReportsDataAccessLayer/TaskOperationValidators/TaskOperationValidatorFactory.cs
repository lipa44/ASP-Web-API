using ReportsDataAccessLayer.TaskOperationValidators.Abstractions;
using ReportsLibrary.Tasks.TaskStates;
using ReportsLibrary.Tools;
using Task = ReportsLibrary.Tasks.Task;

namespace ReportsDataAccessLayer.TaskOperationValidators
{
    public class TaskOperationValidatorFactory : ITaskOperationValidatorFactory
    {
        public ITaskOperationValidator CreateValidator(Task task)
            => task.State switch
            {
                OpenTaskState => new OpenTaskOperationValidator(),
                ActiveTaskState => new ActiveTaskOperationValidator(),
                ResolvedTaskState => new ResolvedTaskOperationValidator(),
                _ => throw new ReportsException("Task state unrecognized")
            };
    }
}