using System;
using Domain.Entities;

namespace Domain.Tasks.TaskChangeCommands;

public class SetTaskStateCommand : ITaskCommand
{
    private readonly Enums.ReportTaskStates _newState;

    public SetTaskStateCommand(Enums.ReportTaskStates reportTaskState)
        => _newState = reportTaskState;

    public void Execute(Employee changer, ReportsTask reportsTaskToChange)
    {
        ArgumentNullException.ThrowIfNull(changer);
        ArgumentNullException.ThrowIfNull(reportsTaskToChange);

        // _task.MakeSnapshot();
        reportsTaskToChange.SetState(changer, _newState);
    }
}