using BookRight.ApplicationLib.Repositories;
using BookRight.DomainLib.Entities.Bookings;
using BookRight.DomainLib.Enums;
using BookRight.InfrastructureLib.Persistance.DbContextFiles;
using Microsoft.EntityFrameworkCore;

namespace BookRight.InfrastructureLib.Repositories.Bookings;

public class BookingRepository(BookRightDbContext db) : IBookingRepository
{
    async Task<Booking?> IBookingRepository.GetByIdAsync(Guid id)
    {
        return await db.Bookings
            .FirstOrDefaultAsync(b => b.Id == id);
    }

    async Task<IReadOnlyList<Booking>> IBookingRepository.GetAllBookingsByIdAsync(Guid id)
    {
        var bookings = await db.Bookings
            .AsNoTracking()
            .ToListAsync();

        return bookings
            .Where(b =>
                b.TherapistId == id ||
                b.ClinicId == id ||
                b.TreatmentId == id ||
                b.Participants.Contains(id))
            .ToList();
    }

    async Task IBookingRepository.AddAsync(Booking booking)
    {
        await db.Bookings.AddAsync(booking);
    }

    async Task IBookingRepository.SaveAsync()
    {
        await db.SaveChangesAsync();
    }

    async Task<decimal> IBookingRepository.GetBookingHistorySumAsync(Guid customerId, int historyMonths)
    {
        var fromDate = DateTime.Now.AddMonths(-historyMonths);

        var bookings = await db.Bookings
            .AsNoTracking()
            .ToListAsync();

        return bookings
            .Where(b =>
                b.Participants.Contains(customerId) &&
                b.Status == BookingStatus.Completed &&
                b.Time.From >= fromDate)
            .Sum(b => b.Price);
    }

    async Task<int> IBookingRepository.GetNumberOfBirthdayDiscountThisYearByIdAsync(Guid customerId, int year)
    {
        var bookings = await db.Bookings
            .AsNoTracking()
            .ToListAsync();

        return bookings
            .Count(b =>
                b.Participants.Contains(customerId) &&
                b.DiscountTypeUsed == DiscountTypes.BirthdayMonth &&
                b.Time.From.Year == year);
    }
}