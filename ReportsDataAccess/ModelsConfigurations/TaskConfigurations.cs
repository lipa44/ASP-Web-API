namespace ReportsDataAccess.ModelsConfigurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReportsDomain.Tasks;

public class TaskConfigurations : IEntityTypeConfiguration<ReportsTask>
{
    public void Configure(EntityTypeBuilder<ReportsTask> builder)
    {
        builder.HasKey(reportsTask => reportsTask.Id);

        builder.OwnsMany(
            task => task.Comments,
            comment =>
                comment.WithOwner());

        builder.OwnsMany(
            task => task.Modifications,
            modification =>
                modification.WithOwner());
    }
}