using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Task = ReportsLibrary.Tasks.Task;

namespace ReportsDataAccessLayer.ModelsConfigurations;

public class TaskConfigurations : IEntityTypeConfiguration<Task>
{
    public void Configure(EntityTypeBuilder<Task> builder)
    {
        builder.HasOne(t => t.Sprint)
            .WithMany(s => s.Tasks)
            .HasForeignKey(t => t.SprintId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.OwnsMany(
            task => task.Comments,
            comment =>
                comment.WithOwner());

        builder.OwnsMany(
            task => task.Modifications,
            modification =>
                modification.WithOwner());

        builder.HasOne(t => t.Sprint)
            .WithMany(s => s.Tasks)
            .HasForeignKey(t => t.SprintId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}