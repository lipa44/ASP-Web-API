using ReportsLibrary.Tasks;
using ReportsLibrary.Tasks.TaskChangeCommands;

namespace ReportsDataAccessLayer.Services.Interfaces
{
    public interface ITaskService
    {
        Task<List<ReportsTask>> GetTasks();
        Task<ReportsTask> FindTaskById(Guid taskId);
        Task<ReportsTask> GetTaskById(Guid taskId);
        void RemoveTaskById(Guid taskId);
        Task<IReadOnlyCollection<ReportsTask>> FindTasksByCreationTime(DateTime creationTime);
        Task<IReadOnlyCollection<ReportsTask>> FindTasksByModificationDate(DateTime modificationTime);
        Task<IReadOnlyCollection<ReportsTask>> FindTasksByEmployeeId(Guid employeeId);
        Task<IReadOnlyCollection<ReportsTask>> FindsTaskModifiedByEmployeeId(Guid employeeId);
        Task<IReadOnlyCollection<ReportsTask>> FindTasksCreatedByEmployeeSubordinates(Guid employeeId);
        Task<ReportsTask> CreateTask(string taskName, Guid creatorId);
        void UseChangeTaskCommand(Guid taskId, Guid changerId, ITaskCommand command);
    }
}