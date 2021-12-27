namespace ReportsDomain.Tasks.TaskChangeCommands;

using ReportsDomain.Employees;

public interface ITaskCommand
{
    void Execute(Employee changer, ReportsTask reportsTaskToChange);
}