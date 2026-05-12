using BookRight.DomainLib.Entities.Therapists;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookRight.InfrastructureLib.Persistance.EF_Configurations;

internal class TherapistConfiguration : IEntityTypeConfiguration<Therapist>
{
    public void Configure(EntityTypeBuilder<Therapist> builder)
    {
        builder.Property(t => t.CertificationTypes)
           .HasConversion<string>();

        builder.ComplexProperty(
            t => t.Address,
            a => a.ToJson());

        builder.ComplexProperty(
            t => t.Email,
            e => e.ToJson());

        builder.ComplexProperty(
            t => t.PhoneNumber,
            p => p.ToJson());
    }
}
