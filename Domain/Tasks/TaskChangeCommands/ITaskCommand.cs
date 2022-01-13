using Domain.Entities;

namespace Domain.Tasks.TaskChangeCommands;

public interface ITaskCommand
{
    void Execute(Employee changer, ReportsTask reportsTaskToChange);
}