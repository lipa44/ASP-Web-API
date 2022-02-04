namespace Domain.Entities.Tasks.TaskChangeCommands;

public interface ITaskCommand
{
    void Execute(Employee changer, ReportsTask reportsTaskToChange);
}