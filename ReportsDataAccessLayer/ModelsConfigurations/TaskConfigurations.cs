using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReportsLibrary.Tasks;

namespace ReportsDataAccessLayer.ModelsConfigurations;

public class TaskConfigurations : IEntityTypeConfiguration<ReportsTask>
{
    public void Configure(EntityTypeBuilder<ReportsTask> builder)
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