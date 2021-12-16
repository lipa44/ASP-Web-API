using ReportsLibrary.Tasks;

namespace ReportsWebApiLayer.DataTransferObjects;

public class FullReportsTaskDto
{
    public string Title { get; init; }
    public string Content { get; init; }

    public string State { get; init; }
    public Guid? OwnerId { get; init; }
    public Guid? SprintId { get; init; }
    public Guid? Id { get; init; }
    public IReadOnlyCollection<TaskModification> TaskModifications { get; init; }
    public IReadOnlyCollection<TaskComment> TaskComments { get; init; }
}