using System;
using ReportsLibrary.Employees;

namespace ReportsLibrary.Tasks.TaskChangeCommands;

public class ChangeTaskContentCommand : IChangeTaskCommand
{
    private readonly Employee _changer;
    private readonly Task _task;
    private readonly string _newContent;

    public ChangeTaskContentCommand(Employee changer, Task task, string content)
    {
        ArgumentNullException.ThrowIfNull(task);

        _changer = changer;
        _task = task;
        _newContent = content;
    }

    public void Execute()
    {
        _task.MakeSnapshot();
        _task.ChangeContent(_changer, _newContent);
    }
}