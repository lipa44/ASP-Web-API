using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.ModelsConfigurations;

public class SprintConfigurations : IEntityTypeConfiguration<Sprint>
{
    public void Configure(EntityTypeBuilder<Sprint> builder)
    {
        builder.HasKey(report => report.Id);

        // one-to-many relationship: sprint has many tasks and task has one sprint
        builder.HasMany(sprint => sprint.Tasks)
            .WithOne()
            .HasForeignKey(t => t.SprintId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}