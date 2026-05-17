using BookRight.ApplicationLib.Repositories;
using BookRight.DomainLib.Exceptions;
using BookRight.DomainLib.Services;
using BookRight.DomainLib.ValueObjects;
using BookRight.FacadeLib.Commands.Booking.DTOs;
using BookRight.FacadeLib.Commands.Booking.Interfaces;

namespace BookRight.ApplicationLib.Handlers.Bookings;

public class ChangeTimeHandler(
    IBookingRepository bookingRepository,
    ICustomerRepository customerRepository,
    ITherapistRepository therapistRepository,
    IValidateOverlapService overlapService) : IChangeTimeHandler
{
    async Task IChangeTimeHandler.Handle(ChangeTimeCommand command)
    {
        var booking = await bookingRepository.GetByIdAsync(command.BookingId)
            ?? throw new NotFoundException("Booking could not be found.");

        var therapist = await therapistRepository.GetByIdAsync(booking.TherapistId)
            ?? throw new NotFoundException("Therapist could not be found.");

        var therapistBookings = await bookingRepository.GetBookingsByTherapistIdAsync(therapist.Id);


        var newTimeSlot = new TimeSlot(command.From, command.To);
        
        overlapService.Validate(booking, therapistBookings);


        foreach (var p in booking.Participants)
        {
            var customer = await customerRepository.GetByIdAsync(p)
            ?? throw new NotFoundException("Customer could not be found.");

            var customerBookings = await bookingRepository.GetBookingsByCustomerIdAsync(customer.Id);

            overlapService.Validate(booking, customerBookings);
        }


        booking.ChangeTime(newTimeSlot);

        await bookingRepository.SaveAsync();
    }
}