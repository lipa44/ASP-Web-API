using System;
using ReportsDomain.Employees;
using ReportsDomain.Tools;

namespace ReportsDomain.Tasks.TaskChangeCommands;

public class SetTaskOwnerCommand : ITaskCommand
{
    public SetTaskOwnerCommand(Guid implementorId)
        => NewImplementorId = implementorId;

    public Guid NewImplementorId { get; init; }
    public Employee NewImplementor { get; set; }

    public void Execute(Employee changer, ReportsTask reportsTaskToChange)
    {
        ArgumentNullException.ThrowIfNull(changer);
        ArgumentNullException.ThrowIfNull(reportsTaskToChange);
        ArgumentNullException.ThrowIfNull(NewImplementor);

        if (NewImplementor.Id != NewImplementorId)
            throw new ReportsException("New task implementor entity must have same Id as given in command");

        // _task.MakeSnapshot();
        reportsTaskToChange.SetOwner(changer, NewImplementor);
    }
}