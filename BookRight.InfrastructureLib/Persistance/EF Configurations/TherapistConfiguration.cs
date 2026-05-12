using BookRight.DomainLib.Entities.Therapists;
using BookRight.DomainLib.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Text.Json;

namespace BookRight.InfrastructureLib.Persistance.EF_Configurations;

internal class TherapistConfiguration : IEntityTypeConfiguration<Therapist>
{
    public void Configure(EntityTypeBuilder<Therapist> builder)
    {
        builder.Property<List<CertificationTypes>>("_certificationTypes")
            .HasColumnName("CertificationTypes")
            .HasConversion(
                v => JsonSerializer.Serialize(
                    v.Select(x => x.ToString()).ToList(),
            (JsonSerializerOptions?)null
        ),
        v => JsonSerializer.Deserialize<List<string>>(v, (JsonSerializerOptions?)null)!
            .Select(Enum.Parse<CertificationTypes>)
            .ToList());

        builder.Property<List<Guid>>("_associatedClinics")
            .HasColumnName("AssociatedClinics")
            .HasConversion(
            v => JsonSerializer.Serialize(
                v.Select(x => x).ToList(),
            (JsonSerializerOptions?)null),

            v => JsonSerializer.Deserialize<List<Guid>>(v, (JsonSerializerOptions?)null)!
            .ToList());

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
