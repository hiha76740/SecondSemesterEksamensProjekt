using BookRight.DomainLib.Entities.Bookings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookRight.InfrastructureLib.Persistance.EF_Configurations;

internal class BookingConfiguration : IEntityTypeConfiguration<Booking>
{
    public void Configure(EntityTypeBuilder<Booking> builder)
    {
        throw new NotImplementedException();
    }
}
