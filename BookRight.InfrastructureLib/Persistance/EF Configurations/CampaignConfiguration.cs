using BookRight.DomainLib.Entities.Campaigns;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

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
    }
}
