using System;
using Reports.Employees.Abstractions;
using Reports.Tools;

namespace Reports.Task
{
    public class TaskComment
    {
        public TaskComment(Employee commentator, string content)
        {
            ArgumentNullException.ThrowIfNull(commentator);

            if (string.IsNullOrWhiteSpace(content))
                throw new ReportsException("Content to add comment is null");

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