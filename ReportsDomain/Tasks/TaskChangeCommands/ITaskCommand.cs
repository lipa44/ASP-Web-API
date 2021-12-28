namespace ReportsDomain.Tasks.TaskChangeCommands;

using Employees;

public interface ITaskCommand
{
    void Execute(Employee changer, ReportsTask reportsTaskToChange);
}