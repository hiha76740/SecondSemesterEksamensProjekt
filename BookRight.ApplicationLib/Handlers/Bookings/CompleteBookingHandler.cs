using BookRight.ApplicationLib.Repositories;
using BookRight.DomainLib.Exceptions;
using BookRight.FacadeLib.Commands.Booking.DTOs;
using BookRight.FacadeLib.Commands.Booking.Interfaces;

namespace BookRight.ApplicationLib.Handlers.Bookings;

public class CompleteBookingHandler(
    IBookingRepository bookingRepository) : ICompleteBookingHandler
{
    async Task ICompleteBookingHandler.Handle(CompleteBookingCommand command)
    {
        var booking = await bookingRepository.GetByIdAsync(command.BookingId)
            ?? throw new NotFoundException("Booking could not be found.");

        booking.Complete();

        await bookingRepository.SaveAsync();
    }
}