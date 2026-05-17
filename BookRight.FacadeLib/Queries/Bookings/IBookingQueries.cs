using BookRight.FacadeLib.DTO;

namespace BookRight.FacadeLib.Queries.Bookings;

public interface IBookingQueries
{
    Task<BookingDTO?> GetByIdAsync(Guid id);
    Task<IReadOnlyList<BookingDTO>> GetAllAsync();
}