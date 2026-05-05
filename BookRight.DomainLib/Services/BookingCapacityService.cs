using BookRight.DomainLib.Entities.Bookings;
using BookRight.DomainLib.Entities.Clinics;
using BookRight.DomainLib.ValueObjects;

namespace BookRight.DomainLib.Services;

/// <summary>
/// Provides functionality to determine whether a new booking can be created for a clinic based on its current capacity
/// and existing bookings.
/// </summary>
/// <remarks>This service evaluates booking capacity by comparing the number of overlapping bookings within a
/// specified time slot to the number of available treatment rooms in the clinic. It is intended to help prevent
/// overbooking scenarios in scheduling workflows.</remarks>
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
