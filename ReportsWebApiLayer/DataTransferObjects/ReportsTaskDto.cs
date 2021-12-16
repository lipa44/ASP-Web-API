namespace ReportsWebApiLayer.DataTransferObjects;

public class ReportsTaskDto
{
    public ReportsTaskDto() { }

    public string Title { get; init; }
    public string Content { get; init; }
    public string State { get; init; }
}