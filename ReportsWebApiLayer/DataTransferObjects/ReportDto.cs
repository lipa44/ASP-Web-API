using ReportsLibrary.Tasks;

namespace ReportsWebApiLayer.DataTransferObjects;

public class ReportDto
{
    public Guid OwnerId { get; init; }
    public Guid WorkTeamId { get; init; }
    public Guid Id { get; init; }
    public IReadOnlyCollection<TaskModification> Modifications { get; init; }
}