using System;
using ReportsLibrary.Employees;
using ReportsLibrary.Tools;

namespace ReportsLibrary.Tasks
{
    public class TaskComment
    {
        public TaskComment() { }

        public TaskComment(Employee commentatorId, string content)
        {
            ArgumentNullException.ThrowIfNull(commentatorId);
            ReportsException.ThrowIfNullOrWhiteSpace(content);

            CommentatorId = commentatorId.Id;
            Content = content;
            CreationTime = DateTime.Now;
        }

        public Guid? CommentatorId { get; init; }
        public string Content { get; init; }
        public DateTime CreationTime { get; init; }
        public Guid Id { get; init; } = Guid.NewGuid();
    }
}