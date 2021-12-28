namespace ReportsDomain.Tasks.TaskChangeCommands;

using System;
using Employees;

public class SetTaskContentCommand : ITaskCommand
{
    private readonly string _newContent;

    public SetTaskContentCommand(string content)
        => _newContent = content;

    public void Execute(Employee changer, ReportsTask reportsTaskToChange)
    {
        ArgumentNullException.ThrowIfNull(changer);
        ArgumentNullException.ThrowIfNull(reportsTaskToChange);

        // _task.MakeSnapshot();
        reportsTaskToChange.SetContent(changer, _newContent);
    }
}