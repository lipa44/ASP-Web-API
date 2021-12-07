using System;
using ReportsLibrary.Employees;
using ReportsLibrary.Tasks;
using ReportsLibrary.Tasks.TaskStates;

namespace Reports.Tools
{
    public class TaskOperationValidator
    {
        public bool IsAbleToChangeContent(Task task, Employee changer, string newTaskContent)
        {
            // if (!TaskState.IsAbleToChangeContent(changer, newTaskContent))
            //     throw new PermissionDeniedException($"{changer} is not able to change content in task {TaskName}");
            return task.TaskState.IsAbleToChangeContent(changer, newTaskContent);
        }

        public bool IsAbleToAddComment(Employee changer, string newComment)
        {
            throw new NotImplementedException();
        }

        public bool IsAbleToAddImplementor(Employee changer, Employee newImplementor)
        {
            throw new NotImplementedException();
        }

        public bool IsAbleToChangeTaskState(Employee changer, ITaskState newTaskState)
        {
            throw new NotImplementedException();
        }
    }
}