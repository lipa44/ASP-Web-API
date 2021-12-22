using ReportsDomain.Employees;

namespace ReportsDomain.Tasks.TaskChangeCommands;

public interface ITaskCommand
{
    void Execute(Employee changer, ReportsTask reportsTaskToChange);
}