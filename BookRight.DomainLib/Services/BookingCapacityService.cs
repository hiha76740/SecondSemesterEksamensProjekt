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

    bool IBookingCapacityService.CanCreateBooking(
        Clinic clinic,
        IEnumerable<Booking> existingBookings,
        TimeSlot timeSlot)
    {
        var overlapping = existingBookings
           .Count(b => b.TimeSlot.OverlapsWith(timeSlot));

        return overlapping < clinic.TreatmentRoomLimit;
    }
}
