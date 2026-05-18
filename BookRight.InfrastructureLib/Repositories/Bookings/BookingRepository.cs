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
        return await db.Bookings.FirstOrDefaultAsync(b => b.Id == id);
    }

    async Task<IReadOnlyList<Booking>> IBookingRepository.GetBookingsByCustomerIdAsync(Guid customerId)
    {
        return await db.Bookings
            .Where(b => b.Participants.Contains(customerId))
            .ToListAsync();
    }

    async Task<IReadOnlyList<Booking>> IBookingRepository.GetBookingsByTherapistIdAsync(Guid therapistId)
    {
        return await db.Bookings
            .Where(b => b.TherapistId == therapistId)
            .ToListAsync();
    }

    async Task<IReadOnlyList<Booking>> IBookingRepository.GetBookingsByClinicIdAsync(Guid clinicId)
    {
        return await db.Bookings
            .Where(b => b.ClinicId == clinicId)
            .ToListAsync();
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
        return await db.Bookings
            .Where(b =>
                b.Participants.Contains(customerId) &&
                b.Status == BookingStatus.Completed &&
                b.Time.From >= DateTime.Now.AddMonths(-historyMonths))
            .SumAsync(b => b.Price);
    }

    async Task<int> IBookingRepository.GetNumberOfBirthdayDiscountThisYearByIdAsync(Guid customerId, int year)
    {
        return await db.Bookings.CountAsync(b =>
            b.Participants.Contains(customerId) &&
            b.Time.From.Year == year &&
            b.DiscountTypeUsed == DiscountTypes.BirthdayMonth);
    }
}