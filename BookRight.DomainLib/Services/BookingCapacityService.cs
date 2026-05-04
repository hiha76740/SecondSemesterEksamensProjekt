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
public sealed class BookingCapacityService
{
    /// <summary>
    /// Determines whether a new booking can be created for the specified clinic and time slot, given the existing
    /// bookings.
    /// </summary>
    /// <remarks>This method does not create a booking; it only checks availability based on the current state
    /// of bookings and the clinic's capacity. The caller is responsible for ensuring that the provided parameters are
    /// valid and up to date.</remarks>
    /// <param name="clinic">The clinic for which the booking is being considered. Must not be null.</param>
    /// <param name="existingBookings">A collection of existing bookings for the clinic. Used to check for overlapping bookings in the specified time
    /// slot. Must not be null.</param>
    /// <param name="timeSlot">The time slot for the proposed booking. Must not be null.</param>
    /// <returns>true if the number of overlapping bookings in the specified time slot is less than the clinic's treatment room
    /// count; otherwise, false.</returns>
    public bool CanCreateBooking(
        Clinic clinic,
        IEnumerable<Booking> existingBookings,
        TimeSlot timeSlot)
    {
        var overlapping = existingBookings
            .Count(b => b.TimeSlot.OverlapsWith(timeSlot));

        return overlapping < clinic.TreatmentRoomLimit;
    }
}
