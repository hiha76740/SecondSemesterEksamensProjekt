using BookRight.DomainLib.Entities.Bookings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

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
    }
}
