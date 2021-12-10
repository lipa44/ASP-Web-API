using System;
using ReportsLibrary.Employees;

namespace ReportsLibrary.Tasks.TaskChangeCommands;

public class ChangeTaskImplementorCommand : IChangeTaskCommand
{
    private readonly Employee _changer;
    private readonly Task _task;
    private readonly Employee _newImplementor;

    public ChangeTaskImplementorCommand(Employee changer, Task task, Employee implementor)
    {
        ArgumentNullException.ThrowIfNull(task);

        _changer = changer;
        _task = task;
        _newImplementor = implementor;
    }

    public void Execute()
    {
        // _task.MakeSnapshot();
        _task.SetImplementer(_changer, _newImplementor);
    }
}