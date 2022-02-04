using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.Tasks.TaskStates;

[NotMapped]
public abstract class TaskState
{
    public Guid Id { get; init; } = Guid.NewGuid();

    public abstract bool IsAbleToChangeContent(Employee changer, string newTaskContent);
    public abstract bool IsAbleToAddComment(Employee changer, string newComment);
    public abstract bool IsAbleToAddImplementor(Employee changer, Employee newImplementor);
    public abstract bool IsAbleToChangeTaskState(Employee changer, TaskState newTaskState);

    public override string ToString() => GetType().Name;
}