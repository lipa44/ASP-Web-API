using System;
using ReportsLibrary.Employees;

namespace ReportsLibrary.Tasks.TaskChangeCommands;

public class SetTaskContentCommand : ITaskCommand
{
    private readonly string _newContent;

    public SetTaskContentCommand(string content)
        => _newContent = content;

    public void Execute(Employee changer, Task taskToChange)
    {
        ArgumentNullException.ThrowIfNull(changer);
        ArgumentNullException.ThrowIfNull(taskToChange);

        // _task.MakeSnapshot();
        taskToChange.ChangeContent(changer, _newContent);
    }
}