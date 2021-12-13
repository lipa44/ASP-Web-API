using ReportsLibrary.Employees;
using ReportsLibrary.Tasks.TaskChangeCommands;
using ReportsLibrary.Tasks.TaskStates;
using Task = ReportsLibrary.Tasks.Task;

namespace ReportsWebApiLayer.DataBase.Services.Interfaces
{
    public interface ITaskService
    {
        Task<List<Task>> GetTasks();
        Task<Task> FindTaskById(Guid taskId);
        Task<Task> GetTaskById(Guid taskId);
        void RemoveTaskById(Guid taskId);
        Task<IReadOnlyCollection<Task>> FindTasksByCreationTime(DateTime creationTime);
        Task<IReadOnlyCollection<Task>> FindTasksByModificationTime(DateTime modificationTime);
        Task<IReadOnlyCollection<Task>> FindTaskByEmployeeId(Guid employeeId);
        Task<IReadOnlyCollection<Task>> FindsTaskModifiedByEmployeeId(Guid employeeId);
        Task<IReadOnlyCollection<Task>> FindTasksCreatedByEmployeeSubordinates(Guid employeeId);
        Task<Task> CreateTask(string taskName, Employee owner, Guid ownerId);
        void UseChangeTaskCommand(Guid taskId, Guid changerId, ITaskCommand command);
        void SetState(Guid taskId, Guid changerId, TaskState newState);
        void SetContent(Guid taskId, Guid changerId, string newContent);
        void AddComment(Guid taskId, Guid commentatorId, string comment);
        System.Threading.Tasks.Task SetOwner(Guid taskId, Guid changerId, Guid newImplementerId);
    }
}