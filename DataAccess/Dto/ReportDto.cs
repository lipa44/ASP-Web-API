namespace DataAccess.Dto;

public record ReportDto
{
    public string OwnerData { get; init; }
    public string State { get; init; }
}