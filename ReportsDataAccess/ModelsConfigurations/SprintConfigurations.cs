namespace ReportsDataAccess.ModelsConfigurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReportsDomain.Entities;

public class SprintConfigurations : IEntityTypeConfiguration<Sprint>
{
    public void Configure(EntityTypeBuilder<Sprint> builder)
    {
        builder.HasKey(report => report.Id);

        builder.HasMany(sprint => sprint.Tasks)
            .WithOne()
            .HasForeignKey(t => t.SprintId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}