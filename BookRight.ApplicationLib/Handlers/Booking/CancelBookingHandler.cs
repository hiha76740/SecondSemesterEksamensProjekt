using BookRight.ApplicationLib.Exceptions;
using BookRight.ApplicationLib.Repositories;
using BookRight.FacadeLib.Booking.DTOs;
using BookRight.FacadeLib.Booking.Interfaces;

namespace BookRight.ApplicationLib.Handlers.Booking;

public class CancelBookingHandler(IBookingRepository bookingRepository) : ICancelBookingHandler
{
    async Task ICancelBookingHandler.Handle(CancelBookingCommand command)
    {
        if (command.BookingId == Guid.Empty)
            throw new BookRight.ApplicationLib.Exceptions.ApplicationException("BookingId cannot be empty.");

        var booking = await bookingRepository.GetByIdAsync(command.BookingId)
            ?? throw new NotFoundException("Booking could not be found.");

        booking.Cancel();

        await bookingRepository.Save();
    }
}