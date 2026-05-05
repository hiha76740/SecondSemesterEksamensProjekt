using BookRight.DomainLib.Entities.Bookings;
using BookRight.DomainLib.Entities.Clinics;
using BookRight.DomainLib.ValueObjects;

namespace BookRight.DomainLib.Services
{
    public interface IBookingCapacityService
    {
        bool CanCreateBooking(Clinic clinic, IEnumerable<Booking> existingBookings, TimeSlot timeSlot);
    }
}