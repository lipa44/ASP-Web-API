namespace ReportsDomain.Tasks.TaskSnapshots;

using Entities;
using System;
using System.Collections.Generic;

public interface ITaskSnapshot
{
    public string Name { get; init; }
    public string Content { get; init; }
    public List<TaskComment> Comments { get; init; }
    public List<TaskModification> Modifications { get; init; }
    public Employee Owner { get; init; }
    public DateTime ModificationTime { get; init; }
    public Enums.ReportTaskStates ReportTaskState { get; init; }
}