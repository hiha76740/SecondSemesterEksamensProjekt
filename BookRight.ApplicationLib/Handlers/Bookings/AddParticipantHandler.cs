using BookRight.ApplicationLib.Repositories;
using BookRight.DomainLib.Exceptions;
using BookRight.FacadeLib.Commands.Booking.DTOs;
using BookRight.FacadeLib.Commands.Booking.Interfaces;

namespace BookRight.ApplicationLib.Handlers.Bookings;

public class AddParticipantHandler(IBookingRepository bookingRepository, ICustomerRepository customerRepository) : IAddParticipantHandler
{
    async Task IAddParticipantHandler.Handle(AddParticipantCommand command)
    {
        var booking = await bookingRepository.GetByIdAsync(command.BookingId)
            ?? throw new NotFoundException("Booking not found");

        _ = await customerRepository.GetByIdAsync(command.CustomerId)
            ?? throw new NotFoundException("Customer not found");

        booking.AddParticipant(command.CustomerId);

        await bookingRepository.SaveAsync();
    }
}
