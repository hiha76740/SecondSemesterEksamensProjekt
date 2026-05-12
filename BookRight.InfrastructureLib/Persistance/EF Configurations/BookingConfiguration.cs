using BookRight.DomainLib.Entities.Bookings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Text.Json;

namespace BookRight.InfrastructureLib.Persistance.EF_Configurations;

internal class BookingConfiguration : IEntityTypeConfiguration<Booking>
{
    public void Configure(EntityTypeBuilder<Booking> builder)
    {
        builder.Property(b => b.Status)
           .HasConversion<string>();

        builder.Property(b => b.DiscountTypeUsed)
           .HasConversion<string>();

        builder.ComplexProperty(
            b => b.Time,
            t => t.ToJson());

        builder.Property<List<Guid>>("_participants")
            .HasColumnName("Participants")
            .HasConversion(
            v => JsonSerializer.Serialize(
                v.Select(x => x).ToList(),
                (JsonSerializerOptions?)null),

            v => JsonSerializer.Deserialize<List<Guid>>(v, (JsonSerializerOptions?)null)!
            .ToList());
    }
}
