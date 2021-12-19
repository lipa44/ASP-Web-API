using ReportsLibrary.Tasks;

namespace ReportsWebApiLayer.DataTransferObjects;

public class FullReportDto
{
    public string OwnerData { get; init; }
    public string WorkTeamName { get; init; }
    public IReadOnlyCollection<TaskModification> Modifications { get; init; }
}