using System;
using Domain.Entities;

namespace Domain.Tasks.TaskChangeCommands;

public class SetTaskSprintCommand : ITaskCommand
{
    private readonly Sprint _newSprint;

    public SetTaskSprintCommand(Sprint sprint)
        => _newSprint = sprint;

    public void Execute(Employee changer, ReportsTask reportsTaskToChange)
    {
        ArgumentNullException.ThrowIfNull(changer);
        ArgumentNullException.ThrowIfNull(reportsTaskToChange);

        // _task.MakeSnapshot();
        reportsTaskToChange.SetSprint(changer, _newSprint);
    }
}