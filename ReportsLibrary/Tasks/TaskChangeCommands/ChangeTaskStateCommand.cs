using System;
using ReportsLibrary.Employees;
using ReportsLibrary.Tasks.TaskStates;

namespace ReportsLibrary.Tasks.TaskChangeCommands;

public class ChangeTaskStateCommand
{
    private readonly Employee _changer;
    private readonly Task _task;
    private readonly TaskState _newState;

    public ChangeTaskStateCommand(Employee changer, Task task, TaskState taskState)
    {
        ArgumentNullException.ThrowIfNull(task);

        _changer = changer;
        _task = task;
        _newState = taskState;
    }

    public void Execute()
    {
        _task.MakeSnapshot();
        _task.ChangeState(_changer, _newState);
    }
}