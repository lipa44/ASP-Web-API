using System;
using ReportsLibrary.Employees;

namespace ReportsLibrary.Tasks.TaskChangeCommands;

public class SetTaskStateCommand : ITaskCommand
{
    private readonly Enums.TaskStates _newState;

    public SetTaskStateCommand(Enums.TaskStates taskState)
        => _newState = taskState;

    public void Execute(Employee changer, ReportsTask reportsTaskToChange)
    {
        ArgumentNullException.ThrowIfNull(changer);
        ArgumentNullException.ThrowIfNull(reportsTaskToChange);

        // _task.MakeSnapshot();
        reportsTaskToChange.SetState(changer, _newState);
    }
}