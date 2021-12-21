namespace ReportsWebApiLayer.DataTransferObjects;

public class TaskModificationDto
{
    public string Data { get; init; }
    public string Action { get; init; }
    public DateTime ModificationTime { get; init; }
}