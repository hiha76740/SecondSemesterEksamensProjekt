using BookRight.ApplicationLib.Repositories;
using BookRight.DomainLib.Exceptions;
using BookRight.DomainLib.Services;
using BookRight.FacadeLib.Commands.Booking.DTOs;
using BookRight.FacadeLib.Commands.Booking.Interfaces;

namespace BookRight.ApplicationLib.Handlers.Bookings;

public class ChangeClinicHandler(
    IBookingRepository bookingRepository,
    IClinicRepository clinicRepository,
    IBookingCapacityService bookingCapacityService) : IChangeClinicHandler
{
    async Task IChangeClinicHandler.Handle(ChangeClinicCommand command)
    {
        var booking = await bookingRepository.GetByIdAsync(command.BookingId)
            ?? throw new NotFoundException("Booking could not be found.");

        var clinic = await clinicRepository.GetByIdAsync(command.ClinicId)
            ?? throw new NotFoundException("Clinic could not be found.");

        var clinicBookings = await bookingRepository.GetAllBookingsByIdAsync(clinic.Id);

        if (bookingCapacityService.CanCreateBooking(clinic, clinicBookings, booking.Time) == false)
            throw new Exceptions.ApplicationException("Clinic capacity was exceeded.");

        booking.ChangeClinic(clinic.Id);

        await bookingRepository.SaveAsync();
    }
}