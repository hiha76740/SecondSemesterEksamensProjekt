using BookRight.DomainLib.Entities.Bookings;

namespace BookRight.ApplicationLib.Repositories;

// Repository-interface: kontrakt for datadgang til Booking.
// Placeres i Application-laget — implementeres i Infrastructure.

public interface IBookingRepository
{
    Task<Booking?> GetByIdAsync(Guid id);

    Task<IReadOnlyList<Booking>> GetAllBookingsByIdAsync(Guid id);

    Task AddAsync(Booking booking);

    Task SaveAsync();
    Task<decimal> GetBookingHistorySumAsync(Guid customerId, int historyMonths);
    Task<int> GetNumberOfBirthdayDiscountThisYearByIdAsync(Guid customerId, int year);
}
