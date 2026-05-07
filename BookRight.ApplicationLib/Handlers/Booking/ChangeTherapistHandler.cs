using BookRight.ApplicationLib.Repositories;
using BookRight.DomainLib.Exceptions;
using BookRight.FacadeLib.Commands.Booking.DTOs;
using BookRight.FacadeLib.Commands.Booking.Interfaces;

namespace BookRight.ApplicationLib.Handlers.Booking;

public class ChangeTherapistHandler(
    IBookingRepository bookingRepository,
    ITherapistRepository therapistRepository) : IChangeTherapistHandler
{
    async Task IChangeTherapistHandler.Handle(ChangeTherapistCommand command)
    {
        if (command.BookingId == Guid.Empty)
            throw new Exceptions.ApplicationException("BookingId cannot be empty.");

        if (command.TherapistId == Guid.Empty)
            throw new Exceptions.ApplicationException("TherapistId cannot be empty.");

        var booking = await bookingRepository.GetByIdAsync(command.BookingId)
            ?? throw new NotFoundException("Booking could not be found.");

        var therapist = await therapistRepository.GetByIdAsync(command.TherapistId)
            ?? throw new NotFoundException("Therapist could not be found.");

        var therapistBookings = await bookingRepository.GetAllBookingsByIdAsync(therapist.Id);

        booking.ChangeTherapist(
            therapist.Id,
            therapistBookings);

        await bookingRepository.Save();
    }
}