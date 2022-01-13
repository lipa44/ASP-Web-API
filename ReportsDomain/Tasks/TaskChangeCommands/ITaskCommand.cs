namespace ReportsDomain.Tasks.TaskChangeCommands;

using Entities;

public interface ITaskCommand
{
    void Execute(Employee changer, ReportsTask reportsTaskToChange);
}