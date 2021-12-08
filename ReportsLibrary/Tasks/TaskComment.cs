using System;
using ReportsLibrary.Employees;
using ReportsLibrary.Tools;

namespace ReportsLibrary.Tasks
{
    public class TaskComment
    {
        public TaskComment() { }

        public TaskComment(Employee commentator, string content)
        {
            ArgumentNullException.ThrowIfNull(commentator);
            ReportsException.ThrowIfNullOrWhiteSpace(content);

            Commentator = commentator;
            Content = content;
            CreationTime = DateTime.Now;
        }

        public Employee Commentator { get; init; }
        public string Content { get; init; }
        public DateTime CreationTime { get; init; }
        public Guid Id { get; init; } = Guid.NewGuid();
    }
}