using System;
using ReportsLibrary.Employees;

namespace ReportsLibrary.Tasks.TaskChangeCommands;

public class AddTaskCommentCommand : ITaskCommand
{
    private readonly string _newComment;

    public AddTaskCommentCommand(string comment)
        => _newComment = comment;

    public void Execute(Employee changer, ReportsTask reportsTaskToChange)
    {
        ArgumentNullException.ThrowIfNull(changer);
        ArgumentNullException.ThrowIfNull(reportsTaskToChange);

        // _task.MakeSnapshot();
        reportsTaskToChange.AddComment(changer, _newComment);
    }
}