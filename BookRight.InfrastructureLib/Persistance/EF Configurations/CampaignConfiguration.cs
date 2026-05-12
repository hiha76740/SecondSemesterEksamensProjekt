using BookRight.DomainLib.Entities.Campaigns;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Text.Json;

namespace BookRight.InfrastructureLib.Persistance.EF_Configurations;

internal class CampaignConfiguration : IEntityTypeConfiguration<Campaign>
{
    public void Configure(EntityTypeBuilder<Campaign> builder)
    {
        builder.Property(c => c.Status)
           .HasConversion<string>();

        builder.ComplexProperty(
            c => c.CampaignPeriod,
            cp => cp.ToJson());

        builder.Property<IReadOnlyList<Guid>>("AssignedTreatments")
            .HasColumnName("AssignedTreatments")
            .HasConversion(
            v => JsonSerializer.Serialize(
                v.Select(x => x).ToList(),
                (JsonSerializerOptions?)null),

            v => JsonSerializer.Deserialize<List<Guid>>(v, (JsonSerializerOptions?)null)!
            .ToList());
    }
}
