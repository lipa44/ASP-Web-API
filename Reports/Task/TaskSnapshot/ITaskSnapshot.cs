#nullable enable
using System;
using System.Collections.Generic;

namespace Reports.Task.TaskSnapshot
{
    public interface ITaskSnapshot
    {
        string GetName();
        string? GetContent();
        List<TaskComment> GetComments();
        DateTime GetModificationTime();
        TaskState GetTaskState();
    }
}