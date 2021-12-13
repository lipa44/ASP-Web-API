using System;
using ReportsLibrary.Employees;

namespace ReportsLibrary.Tasks.TaskChangeCommands;

public class SetTaskTitleCommand : ITaskCommand
{
    private readonly string _newName;

    public SetTaskTitleCommand(string name)
    {
        _newName = name;
    }

    public void Execute(Employee changer, Task taskToChange)
    {
        ArgumentNullException.ThrowIfNull(changer);
        ArgumentNullException.ThrowIfNull(taskToChange);

        // _task.MakeSnapshot();
        taskToChange.ChangeName(changer, _newName);
    }
}