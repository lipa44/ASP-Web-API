using Reports.Employees;
using Reports.Tools;

namespace Reports.Task
{
    public class TaskComment
    {
        public TaskComment(Employee commentator, string content)
        {
            if (commentator is null)
                throw new ReportsException("Commentator to add comment is null");

            if (string.IsNullOrWhiteSpace(content))
                throw new ReportsException("Content to add comment is null");

            Commentator = commentator;
            Content = content;
        }

        public Employee Commentator { get; }
        public string Content { get; }
    }
}