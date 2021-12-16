using System;
using ReportsLibrary.Employees;

namespace ReportsLibrary.Tasks.TaskChangeCommands;

public class SetTaskTitleCommand : ITaskCommand
{
    private readonly string _newName;

    public SetTaskTitleCommand(string name)
        => _newName = name;

    public void Execute(Employee changer, ReportsTask reportsTaskToChange)
    {
        ArgumentNullException.ThrowIfNull(changer);
        ArgumentNullException.ThrowIfNull(reportsTaskToChange);

        // _task.MakeSnapshot();
        reportsTaskToChange.ChangeName(changer, _newName);
    }
}