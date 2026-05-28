using BookRight.ApplicationLib.Repositories;
using BookRight.DomainLib.Exceptions;
using BookRight.FacadeLib.Commands.Booking.DTOs;
using BookRight.FacadeLib.Commands.Booking.Interfaces;

namespace BookRight.ApplicationLib.Handlers.Bookings;

public class RemoveParticipantHandler(IBookingRepository bookingRepository, ICustomerRepository customerRepository) : IRemoveParticipantHandler
{
    async Task IRemoveParticipantHandler.Handle(RemoveParticipantCommand command)
    {
        var booking = await bookingRepository.GetByIdAsync(command.BookingId)
            ?? throw new NotFoundException("Booking not found");

        _ = await customerRepository.GetByIdAsync(command.CustomerId)
            ?? throw new NotFoundException("Customer not found");

        booking.RemoveParticipant(command.CustomerId);

        await bookingRepository.SaveAsync();
    }
}
