using ReportsLibrary.Employees;
using ReportsLibrary.Tasks;
using ReportsLibrary.Tasks.TaskSnapshots;
using ReportsLibrary.Tasks.TaskStates;
using Task = ReportsLibrary.Tasks.Task;

namespace ReportsWebApiLayer.DataBase.Dto;

public class TaskDto
{
    private readonly List<TaskSnapshot> _snapshots;
    private List<TaskModification> _modifications;
    private List<TaskComment> _comments ;

    public TaskDto(Task taskToCopyFrom)
    {
        ArgumentNullException.ThrowIfNull(taskToCopyFrom);

        Name = taskToCopyFrom.Name;
        Content = taskToCopyFrom.Content;
        CreationTime = taskToCopyFrom.CreationTime;
        ModificationTime = taskToCopyFrom.ModificationTime;
        Implementer = taskToCopyFrom.Implementer;
        TaskState = taskToCopyFrom.TaskState;
        Id = taskToCopyFrom.Id;
        _snapshots = taskToCopyFrom.Snapshots.ToList();
        _modifications = taskToCopyFrom.Modifications.ToList();
        _comments = taskToCopyFrom.Comments.ToList();
    }

    public string Name { get; }
    public string? Content { get; }
    public DateTime CreationTime { get; }
    public DateTime ModificationTime { get; }
    public Employee? Implementer { get; }
    public ITaskState TaskState { get; }
    public Guid Id { get; }
}