using Microsoft.AspNetCore.Server.Kestrel.Core;
using ReportsLibrary.Tools;

namespace ReportsWebApiLayer.DataTransferObjects;

public class ReportsTaskDto
{
    public ReportsTaskDto() { }

    public string Title { get; init; }
    public string Content { get; init; }

    public TaskStates State { get; init; }
    public Guid? OwnerId { get; init; }
    public Guid? SprintId { get; init; }
    public Guid? Id { get; init; }
}