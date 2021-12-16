using ReportsLibrary.Employees;
using ReportsLibrary.Tasks;
using ReportsLibrary.Tasks.TaskChangeCommands;
using ReportsLibrary.Tasks.TaskStates;

namespace ReportsDataAccessLayer.Services.Interfaces
{
    public interface ITaskService
    {
        Task<List<ReportsTask>> GetTasks();
        Task<ReportsTask> FindTaskById(Guid taskId);
        Task<ReportsTask> GetTaskById(Guid taskId);
        void RemoveTaskById(Guid taskId);
        Task<IReadOnlyCollection<ReportsTask>> FindTasksByCreationTime(DateTime creationTime);
        Task<IReadOnlyCollection<ReportsTask>> FindTasksByModificationTime(DateTime modificationTime);
        Task<IReadOnlyCollection<ReportsTask>> FindTaskByEmployeeId(Guid employeeId);
        Task<IReadOnlyCollection<ReportsTask>> FindsTaskModifiedByEmployeeId(Guid employeeId);
        Task<IReadOnlyCollection<ReportsTask>> FindTasksCreatedByEmployeeSubordinates(Guid employeeId);
        Task<ReportsTask> CreateTask(string taskName, Employee owner, Guid ownerId);
        void UseChangeTaskCommand(Guid taskId, Guid changerId, ITaskCommand command);
        void SetState(Guid taskId, Guid changerId, TaskState newState);
        void SetContent(Guid taskId, Guid changerId, string newContent);
        void AddComment(Guid taskId, Guid commentatorId, string comment);
        System.Threading.Tasks.Task SetOwner(Guid taskId, Guid changerId, Guid newImplementerId);
    }
}