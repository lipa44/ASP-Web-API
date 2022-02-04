using Domain.Entities.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.ModelsConfigurations;

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