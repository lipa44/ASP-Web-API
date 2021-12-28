namespace ReportsDomain.Tasks;

using Employees;

public interface ITask
{
    public void SetContent(Employee changer, string newContent);
    public void AddComment(Employee changer, string comment);
    public void SetOwner(Employee changer, Employee newImplementer);
    public void SetState(Employee changer, Enums.TaskStates newState);

    // public void MakeSnapshot();
    // public void RestorePreviousSnapshot();
    // public void RestoreNextSnapshot();
}