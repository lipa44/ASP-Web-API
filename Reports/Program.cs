using Reports.Employees;
using Reports.Task.TaskSnapshot;

namespace Reports
{
    public class Program
    {
        public static void Main()
        {
            var misha = new TeamLead("Misha", "Libchenko");
            var task = new Task.Task("Write reports");
            task.ChangeContent(misha, "Sooo... I have to complete my OOP lab");
            task.StartTask(misha);

            ITaskSnapshot snap = task.MakeSnapshot();
            task.ChangeName(misha, "aboba");
            task.ResolveTask(misha);

            task.RestoreSnapshot(snap);
        }
    }
}