using BookRight.ApplicationLib.Repositories;
using BookRight.DomainLib.Exceptions;
using BookRight.FacadeLib.Commands.Booking.DTOs;
using BookRight.FacadeLib.Commands.Booking.Interfaces;

namespace BookRight.ApplicationLib.Handlers.Bookings;

public class CustomerArrivedHandler(
    IBookingRepository bookingRepository) : ICustomerArrivedHandler
{
    async Task ICustomerArrivedHandler.Handle(CustomerArrivedCommand command)
    {
        var booking = await bookingRepository.GetByIdAsync(command.BookingId)
            ?? throw new NotFoundException("Booking could not be found.");

        booking.Arrived();

        await bookingRepository.SaveAsync();
    }
}