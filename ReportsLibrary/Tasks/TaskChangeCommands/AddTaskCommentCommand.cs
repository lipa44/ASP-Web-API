using System;
using ReportsLibrary.Employees;

namespace ReportsLibrary.Tasks.TaskChangeCommands;

public class AddTaskCommentCommand : ITaskCommand
{
    private readonly string _newComment;

    public AddTaskCommentCommand(string comment)
        => _newComment = comment;

    public void Execute(Employee changer, Task taskToChange)
    {
        ArgumentNullException.ThrowIfNull(changer);
        ArgumentNullException.ThrowIfNull(taskToChange);

        // _task.MakeSnapshot();
        taskToChange.AddComment(changer, _newComment);
    }
}