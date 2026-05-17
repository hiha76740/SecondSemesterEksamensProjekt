using BookRight.FacadeLib.DTO;
using BookRight.FacadeLib.Queries.Bookings;
using BookRight.InfrastructureLib.Persistance.DbContextFiles;
using Microsoft.EntityFrameworkCore;

namespace BookRight.InfrastructureLib.QueryHandlers.Bookings;

public class BookingQueryHandlerIMPL(BookRightDbContext db) : IBookingQueries
{
    async Task<IReadOnlyList<BookingDTO>> IBookingQueries.GetAllAsync()
    {
        return await db.Bookings
            .AsNoTracking()
            .OrderBy(b => b.Time.From)
            .Select(b => new BookingDTO(
                b.Id,
                b.TreatmentId,
                b.TherapistId,
                b.ClinicId,
                b.Time.From,
                b.Time.To,
                b.Price,
                b.ParticipantLimit,
                b.Participants.ToList(),
                b.Status.ToString(),
                b.DiscountTypeUsed.ToString()
                ))
            .ToListAsync();
    }

    async Task<BookingDTO?> IBookingQueries.GetByIdAsync(Guid id)
    {
        return await db.Bookings
            .AsNoTracking()
            .Where(b => b.Id == id)
            .Select(b => new BookingDTO(
                b.Id,
                b.TreatmentId,
                b.TherapistId,
                b.ClinicId,
                b.Time.From,
                b.Time.To,
                b.Price,
                b.ParticipantLimit,
                b.Participants.ToList(),
                b.Status.ToString(),
                b.DiscountTypeUsed.ToString()
                ))
            .FirstOrDefaultAsync();
    }
}