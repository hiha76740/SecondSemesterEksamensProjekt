using BookRight.DomainLib.Entities.Therapists;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookRight.InfrastructureLib.Persistance.EF_Configurations;

internal class TherapistConfiguration : IEntityTypeConfiguration<Therapist>
{
    public void Configure(EntityTypeBuilder<Therapist> builder)
    {
        throw new NotImplementedException();
    }
}
