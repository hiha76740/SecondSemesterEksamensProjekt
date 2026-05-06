using BookRight.ApplicationLib.Exceptions;
using BookRight.ApplicationLib.Repositories;
using BookRight.DomainLib.ValueObjects;
using BookRight.FacadeLib.Booking.DTOs;
using BookRight.FacadeLib.Booking.Interfaces;

namespace BookRight.ApplicationLib.Handlers.Booking;

public class ChangeTimeHandler(
    IBookingRepository bookingRepository,
    ICustomerRepository customerRepository,
    ITherapistRepository therapistRepository) : IChangeTimeHandler
{
    async Task IChangeTimeHandler.Handle(ChangeTimeCommand command)
    {
        if (command.BookingId == Guid.Empty)
            throw new BookRight.ApplicationLib.Exceptions.ApplicationException("BookingId cannot be empty.");

        var booking = await bookingRepository.GetByIdAsync(command.BookingId)
            ?? throw new NotFoundException("Booking could not be found.");

        var customer = await customerRepository.GetByIdAsync(booking.CustomerId)
            ?? throw new NotFoundException("Customer could not be found.");

        var therapist = await therapistRepository.GetByIdAsync(booking.TherapistId)
            ?? throw new NotFoundException("Therapist could not be found.");

        var newTimeSlot = new TimeSlot(command.From, command.To);

        var customerBookings = await bookingRepository.GetAllBookingsByIdAsync(customer.Id);

        var therapistBookings = await bookingRepository.GetAllBookingsByIdAsync(therapist.Id);

        booking.ChangeTime(newTimeSlot, customerBookings, therapistBookings);

        await bookingRepository.Save();
    }
}