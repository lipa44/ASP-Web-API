namespace ReportsDomain.Tasks;

using System;
using Enums;
using Tools;

public record TaskModification
{
    public TaskModification() { }

    public TaskModification(Guid changerId, string changerData, object data, TaskModificationActions action, DateTime modificationTime)
    {
        ArgumentNullException.ThrowIfNull(data);

        if (modificationTime == default)
            throw new ReportsException("Task change time can't be default");

        ChangerId = changerId;
        ChangerData = changerData;
        Data = data.ToString();
        Action = action;
        ModificationTime = modificationTime;
    }

    public Guid ChangerId { get; init; }
    public string ChangerData { get; init; }
    public string Data { get; init; }
    public TaskModificationActions Action { get; init; }
    public DateTime ModificationTime { get; init; }
    public Guid Id { get; init; } = Guid.NewGuid();
}