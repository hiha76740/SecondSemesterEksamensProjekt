using BookRight.DomainLib.Entities.Bookings;
using BookRight.DomainLib.Entities.Clinics;
using BookRight.DomainLib.ValueObjects;

namespace BookRight.DomainLib.Services;

/// <summary>
/// Provides functionality to determine whether a new booking can be created for a clinic based on existing bookings and
/// treatment room limits.
/// </summary>
/// <remarks>This service enforces booking capacity constraints for clinics by evaluating the number of
/// overlapping bookings within a specified time slot. It is typically used to prevent overbooking beyond the clinic's
/// available treatment rooms.</remarks>
public sealed class BookingCapacityService : IBookingCapacityService
{
    /// <summary>
    /// Returns true when the clinic has available treatment-room capacity for a booking at the specified time slot.
    /// </summary>
    /// <remarks>Overlap is determined by Booking.Time.OverlapsWith. Callers should consider concurrency and
    /// external validation when creating bookings.</remarks>
    /// <param name="clinic">Clinic whose TreatmentRoomLimit is used to determine available capacity.</param>
    /// <param name="existingBookings">Enumerable of existing bookings used to count those that overlap the specified time slot.</param>
    /// <param name="timeSlot">Time slot for the requested booking.</param>
    /// <returns>True if the number of bookings overlapping the time slot is less than clinic.TreatmentRoomLimit; otherwise
    /// false.</returns>
    bool IBookingCapacityService.CanCreateBooking(
        Clinic clinic,
        IEnumerable<Booking> existingBookings,
        TimeSlot timeSlot)
    {
        var overlapping = existingBookings
           .Count(b => b.Time.OverlapsWith(timeSlot));

        return overlapping < clinic.TreatmentRoomLimit;
    }
}
