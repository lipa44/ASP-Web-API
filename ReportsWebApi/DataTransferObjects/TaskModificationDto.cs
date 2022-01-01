namespace ReportsWebApi.DataTransferObjects;

public record TaskModificationDto
{
    public string Data { get; init; }
    public string Action { get; init; }
    public DateTime ModificationTime { get; init; }
}