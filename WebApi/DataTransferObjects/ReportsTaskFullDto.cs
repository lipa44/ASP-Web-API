using Domain.Tasks;

namespace WebApi.DataTransferObjects;

public record ReportsTaskFullDto
{
    public string Title { get; init; }
    public string Content { get; init; }

    public string State { get; init; } public Guid? Id { get; init; }
    public IReadOnlyCollection<TaskModificationDto> TaskModifications { get; init; }
    public IReadOnlyCollection<TaskComment> TaskComments { get; init; }
}