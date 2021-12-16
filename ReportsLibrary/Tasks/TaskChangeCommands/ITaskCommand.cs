using ReportsLibrary.Employees;

namespace ReportsLibrary.Tasks.TaskChangeCommands;

public interface ITaskCommand
{
    void Execute(Employee changer, ReportsTask reportsTaskToChange);
}