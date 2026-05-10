using BookRight.DomainLib.Entities.Bookings;
using BookRight.DomainLib.Entities.Clinics;
using BookRight.DomainLib.ValueObjects;

namespace BookRight.DomainLib.Services;

public interface IBookingCapacityService
{
    /// <summary>
    /// Determines whether a new booking can be created for the specified clinic and time slot, given the existing
    /// bookings.
    /// </summary>
    /// <remarks>This method does not create a booking; it only checks whether creating one is
    /// possible based on the current state. The result may depend on clinic-specific rules or booking
    /// constraints.</remarks>
    /// <param name="clinic">The clinic for which the booking is to be created. Cannot be null.</param>
    /// <param name="existingBookings">A collection of existing bookings for the clinic. Used to check for conflicts or availability. Cannot be
    /// null.</param>
    /// <param name="timeSlot">The time slot for the proposed booking. Cannot be null.</param>
    /// <returns>true if a booking can be created for the specified clinic and time slot; otherwise, false.</returns>
    bool CanCreateBooking(Clinic clinic, IEnumerable<Booking> existingBookings, TimeSlot timeSlot);
}