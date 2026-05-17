using BookRight.DomainLib.Entities.Bookings;

namespace BookRight.ApplicationLib.Repositories;

// Repository-interface: kontrakt for datadgang til Booking.
// Placeres i Application-laget — implementeres i Infrastructure.

public interface IBookingRepository
{
    Task<Booking?> GetByIdAsync(Guid id);

    Task<IReadOnlyList<Booking>> GetBookingsByCustomerIdAsync(Guid customerId);

    Task<IReadOnlyList<Booking>> GetBookingsByTherapistIdAsync(Guid therapistId);

    Task<IReadOnlyList<Booking>> GetBookingsByClinicIdAsync(Guid clinicId);

    Task AddAsync(Booking booking);

    Task SaveAsync();

    Task<decimal> GetBookingHistorySumAsync(Guid customerId, int historyMonths);

    Task<int> GetNumberOfBirthdayDiscountThisYearByIdAsync(Guid customerId, int year);
}