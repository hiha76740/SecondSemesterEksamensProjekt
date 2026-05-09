using BookRight.DomainLib.Entities.Bookings;
using BookRight.DomainLib.Exceptions;

namespace BookRight.DomainLib.Services;

public class ValidateOverlapService : IValidateOverlapService
{
    void IValidateOverlapService.Validate(Booking booking, IEnumerable<Booking> bookings)
    {
        bool overlapExsists = bookings.Any(b =>
            b.Id != booking.Id &&
            b.IsActive == true &&
            b.Time.OverlapsWith(booking.Time));

        if (overlapExsists == true)
            throw new DomainException("Overlap in time.");
    }
}
