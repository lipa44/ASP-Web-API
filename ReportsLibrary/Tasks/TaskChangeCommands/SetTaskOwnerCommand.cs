using System;
using ReportsLibrary.Employees;
using ReportsLibrary.Tools;

namespace ReportsLibrary.Tasks.TaskChangeCommands;

public class SetTaskOwnerCommand : ITaskCommand
{
    public SetTaskOwnerCommand(Guid implementorId)
        => NewImplementorId = implementorId;

    public Guid NewImplementorId { get; init; }
    public Employee NewImplementor { get; set; }

    public void Execute(Employee changer, Task taskToChange)
    {
        ArgumentNullException.ThrowIfNull(changer);
        ArgumentNullException.ThrowIfNull(taskToChange);
        ArgumentNullException.ThrowIfNull(NewImplementor);

        if (NewImplementor.Id != NewImplementorId)
            throw new ReportsException("New task implementor entity must have same Id as given in command");

        // _task.MakeSnapshot();
        taskToChange.SetOwner(changer, NewImplementor);
    }
}