using System;
using Domain.Entities;
using Domain.Tools;

namespace Domain.Tasks;

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

    public override bool Equals(object obj) => Equals(obj as TaskComment);
    public override int GetHashCode() => HashCode.Combine(Id, CommentatorId, Content);

    private bool Equals(TaskComment taskComment) => taskComment is not null && taskComment.Id == Id
        && taskComment.CommentatorId == CommentatorId
        && taskComment.Content == Content;
}