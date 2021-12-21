using ReportsLibrary.Tasks;
using ReportsLibrary.Tasks.TaskChangeCommands;

namespace ReportsDataAccessLayer.Services.Interfaces
{
    public interface ITasksService
    {
        Task<List<ReportsTask>> GetTasks();
        Task<ReportsTask> FindTaskById(Guid taskId);
        Task<ReportsTask> GetTaskById(Guid taskId);
        Task<ReportsTask> CreateTask(string taskName);
        Task<ReportsTask> RemoveTaskById(Guid taskId);
        Task<ReportsTask> UseChangeTaskCommand(Guid taskId, Guid changerId, ITaskCommand command);
        Task<IReadOnlyCollection<ReportsTask>> FindTasksByCreationTime(DateTime creationTime);
        Task<IReadOnlyCollection<ReportsTask>> FindTasksByModificationDate(DateTime modificationTime);
        Task<IReadOnlyCollection<ReportsTask>> FindTasksByEmployeeId(Guid employeeId);
        Task<IReadOnlyCollection<ReportsTask>> FindsTaskModifiedByEmployeeId(Guid employeeId);
        Task<IReadOnlyCollection<ReportsTask>> FindTasksCreatedByEmployeeSubordinates(Guid employeeId);
    }
}