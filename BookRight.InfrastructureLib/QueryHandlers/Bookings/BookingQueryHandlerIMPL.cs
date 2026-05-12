using BookRight.DomainLib.Entities.Bookings;
using BookRight.FacadeLib.DTO;
using BookRight.FacadeLib.Queries.Bookings;
using BookRight.InfrastructureLib.Persistance.DbContextFiles;
using Microsoft.EntityFrameworkCore;

namespace BookRight.InfrastructureLib.QueryHandlers.Bookings;

public class BookingQueryHandlerIMPL(BookRightDbContext db) : IBookingQueries
{
    async Task<IReadOnlyList<BookingDTO>> IBookingQueries.GetAllAsync()
    {
        var bookings = await db.Bookings
            .AsNoTracking()
            .ToListAsync();

        return bookings
            .OrderBy(b => b.Time.From)
            .Select(MapToDTO)
            .ToList();
    }

    async Task<BookingDTO?> IBookingQueries.GetByIdAsync(Guid id)
    {
        var booking = await db.Bookings
            .AsNoTracking()
            .FirstOrDefaultAsync(b => b.Id == id);

        if (booking == null)
            return null;

        return MapToDTO(booking);
    }

    private static BookingDTO MapToDTO(Booking booking)
    {
        return new BookingDTO(
            booking.Id,
            booking.TreatmentId,
            booking.TherapistId,
            booking.ClinicId,
            booking.Time.From,
            booking.Time.To,
            booking.Price,
            booking.ParticipantLimit,
            booking.Participants.ToList(),
            booking.Status.ToString(),
            booking.IsActive,
            booking.DiscountTypeUsed.ToString()
            );
    }
}