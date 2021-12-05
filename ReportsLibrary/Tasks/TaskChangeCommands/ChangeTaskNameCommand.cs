using System;
using ReportsLibrary.Employees;

namespace ReportsLibrary.Tasks.TaskChangeCommands;

public class ChangeTaskNameCommand : IChangeTaskCommand
{
    private readonly Employee _changer;
    private readonly Task _task;
    private readonly string _newName;

    public ChangeTaskNameCommand(Employee changer, Task task, string name)
    {
        ArgumentNullException.ThrowIfNull(task);

        _changer = changer;
        _task = task;
        _newName = name;
    }

    public void Execute()
    {
        _task.MakeSnapshot();
        _task.ChangeName(_changer, _newName);
    }
}