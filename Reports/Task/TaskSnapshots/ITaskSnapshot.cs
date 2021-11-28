#nullable enable
using System;
using System.Collections.Generic;
using Reports.Employees.Abstractions;
using Reports.Task.TaskStates;

namespace Reports.Task.TaskSnapshots
{
    public interface ITaskSnapshot
    {
        string GetName();
        string? GetContent();
        List<TaskComment> GetComments();
        List<Employee> GetImplementors();
        DateTime GetModificationTime();
        TaskState GetTaskState();
    }
}