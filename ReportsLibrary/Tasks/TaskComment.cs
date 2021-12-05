using System;
using ReportsLibrary.Employees;
using ReportsLibrary.Tools;

namespace ReportsLibrary.Tasks
{
    public class TaskComment
    {
        public TaskComment(Employee commentator, string content)
        {
            ArgumentNullException.ThrowIfNull(commentator);
            ReportsException.ThrowIfNullOrWhiteSpace(content);

            Commentator = commentator;
            Content = content;
            CreationTime = DateTime.Now;
        }

        public Employee Commentator { get; }
        public string Content { get; }
        public DateTime CreationTime { get; }
        public Guid Id { get; } = Guid.NewGuid();
    }
}