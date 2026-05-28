using BookRight.DomainLib.Entities.Bookings;
using BookRight.DomainLib.Exceptions;

namespace BookRight.DomainLib.Services;

/// <summary>
/// Validates that a booking does not overlap any active booking in a collection.
/// </summary>
/// <remarks>Ignores bookings with the same Id to avoid self-comparison. Uses Booking.Time.OverlapsWith to
/// determine overlaps.</remarks>
public class ValidateOverlapService : IValidateOverlapService
{
    /// <summary>
    /// Validates that booking does not overlap any active booking in bookings.
    /// </summary>
    /// <remarks>Ignores bookings with the same Id to avoid self-comparison. Uses Booking.Time.OverlapsWith to
    /// determine overlaps.</remarks>
    /// <param name="booking">Booking to validate for overlaps.</param>
    /// <param name="bookings">Collection of bookings to check; if null, no overlap checks are performed.</param>
    /// <exception cref="DomainException">Thrown when an active booking in bookings overlaps the specified booking.</exception>
    void IValidateOverlapService.Validate(Booking booking, IEnumerable<Booking> bookings)
    {
        if (bookings != null)
        {
            bool overlapExsists = bookings.Any(b =>
            b.Id != booking.Id &&
            b.IsActive == true &&
            b.Time.OverlapsWith(booking.Time));

            if (overlapExsists == true)
                throw new DomainException("Overlap found.");
        }
        
    }
}
