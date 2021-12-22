using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReportsDomain.Tasks;

namespace ReportsDataAccess.ModelsConfigurations;

public class TaskConfigurations : IEntityTypeConfiguration<ReportsTask>
{
    public void Configure(EntityTypeBuilder<ReportsTask> builder)
    {
        builder.HasOne(task => task.Sprint)
            .WithMany(sprint => sprint.Tasks)
            .HasForeignKey(task => task.SprintId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.OwnsMany(
            task => task.Comments,
            comment =>
                comment.WithOwner());

        builder.OwnsMany(
            task => task.Modifications,
            modification =>
                modification.WithOwner());

        builder.HasOne(task => task.Sprint)
            .WithMany(sprint => sprint.Tasks)
            .HasForeignKey(task => task.SprintId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}