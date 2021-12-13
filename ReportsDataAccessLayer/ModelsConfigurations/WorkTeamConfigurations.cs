using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReportsLibrary.Entities;

namespace ReportsDataAccessLayer.ModelsConfigurations;

public class WorkTeamConfigurations : IEntityTypeConfiguration<WorkTeam>
{
    public void Configure(EntityTypeBuilder<WorkTeam> builder)
    {
        builder.HasOne(team => team.TeamLead)
            .WithOne()
            .HasForeignKey<WorkTeam>(team => team.TeamLeadId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}