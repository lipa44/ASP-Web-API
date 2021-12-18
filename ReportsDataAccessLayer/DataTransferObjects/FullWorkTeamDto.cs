using ReportsLibrary.Employees;
using ReportsLibrary.Entities;

namespace ReportsDataAccessLayer.DataTransferObjects;

public class FullWorkTeamDto
{
    public string Name { get; init; }
    public Guid Id { get; init; }
    public Guid? TeamLeadId { get; init; }
    public Guid? ReportId { get; init; }
    public Report Report { get; init; }
    public IEnumerable<Sprint> Sprints { get; init; }
    public IEnumerable<Employee> Employees { get; init; }
}