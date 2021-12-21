using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;
using ReportsLibrary.Entities;
using ReportsLibrary.Tasks;

namespace ReportsDataAccessLayer.ModelsConfigurations;

public class WorkTeamConfigurations : IEntityTypeConfiguration<WorkTeam>
{
    public void Configure(EntityTypeBuilder<WorkTeam> builder)
    {
        builder.HasOne(team => team.TeamLead)
            .WithOne()
            .HasForeignKey<WorkTeam>(team => team.TeamLeadId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.Property(workTeam => workTeam.Reports)
            .HasConversion(
                v => JsonConvert.SerializeObject(v),
                v => JsonConvert.DeserializeObject<List<Report>>(v));
    }
}