using ReportsLibrary.Employees;
using ReportsLibrary.Tasks;
using ReportsLibrary.Tasks.TaskStates;

namespace ReportsDataAccessLayer.Tools
{
    public class TaskOperationValidator
    {
        public bool IsAbleToChangeContent(ReportsTask reportsTask, Employee changer, string newTaskContent)
        {
            // if (!TaskState.IsAbleToChangeContent(changer, newTaskContent))
            //     throw new PermissionDeniedException($"{changer} is not able to change content in task {TaskName}");
            return reportsTask.State.IsAbleToChangeContent(changer, newTaskContent);
        }

        public bool IsAbleToAddComment(Employee changer, string newComment)
        {
            throw new NotImplementedException();
        }

        public bool IsAbleToAddImplementor(Employee changer, Employee newImplementor)
        {
            throw new NotImplementedException();
        }

        public bool IsAbleToChangeTaskState(Employee changer, TaskState newTaskState)
        {
            throw new NotImplementedException();
        }
    }
}