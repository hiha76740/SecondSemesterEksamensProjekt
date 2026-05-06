using BookRight.ApplicationLib.Exceptions;
using BookRight.ApplicationLib.Repositories;
using BookRight.FacadeLib.Booking.DTOs;
using BookRight.FacadeLib.Booking.Interfaces;

namespace BookRight.ApplicationLib.Handlers.Booking;

public class RegisterNoShowHandler(IBookingRepository bookingRepository) : IRegisterNoShowHandler
{
    async Task IRegisterNoShowHandler.Handle(RegisterNoShowCommand command)
    {
        if (command.BookingId == Guid.Empty)
            throw new BookRight.ApplicationLib.Exceptions.ApplicationException("BookingId cannot be empty.");

        var booking = await bookingRepository.GetByIdAsync(command.BookingId)
            ?? throw new NotFoundException("Booking could not be found.");

        booking.NoShow();

        await bookingRepository.Save();
    }
}