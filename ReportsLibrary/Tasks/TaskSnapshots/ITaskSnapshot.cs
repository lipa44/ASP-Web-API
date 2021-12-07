#nullable enable
using System;
using System.Collections.Generic;
using ReportsLibrary.Employees;
using ReportsLibrary.Tasks.TaskStates;

namespace ReportsLibrary.Tasks.TaskSnapshots
{
    public interface ITaskSnapshot
    {
        string GetName();
        string? GetContent();
        List<TaskComment> GetComments();
        List<TaskModification> GetModifications();
        Employee? GetImplementer();
        DateTime GetModificationTime();
        ITaskState GetTaskState();
    }
}