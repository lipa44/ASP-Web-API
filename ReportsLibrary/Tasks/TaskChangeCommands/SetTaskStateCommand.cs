using System;
using ReportsLibrary.Employees;
using ReportsLibrary.Tasks.TaskStates;

namespace ReportsLibrary.Tasks.TaskChangeCommands;

public class SetTaskStateCommand : ITaskCommand
{
    private readonly TaskState _newState;

    public SetTaskStateCommand(TaskState taskState)
        => _newState = taskState;

    public void Execute(Employee changer, ReportsTask reportsTaskToChange)
    {
        ArgumentNullException.ThrowIfNull(changer);
        ArgumentNullException.ThrowIfNull(reportsTaskToChange);

        // _task.MakeSnapshot();
        reportsTaskToChange.SetState(changer, _newState);
    }
}