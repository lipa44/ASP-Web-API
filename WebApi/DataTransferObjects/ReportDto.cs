namespace WebApi.DataTransferObjects;

public record ReportDto
{
    public string OwnerData { get; init; }
    public string State { get; init; }
}