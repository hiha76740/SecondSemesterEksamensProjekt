using BookRight.ApplicationLib.Repositories;
using BookRight.DomainLib.Exceptions;
using BookRight.FacadeLib.Commands.Booking.DTOs;
using BookRight.FacadeLib.Commands.Booking.Interfaces;

namespace BookRight.ApplicationLib.Handlers.Bookings;

public class ChangeTherapistHandler(
    IBookingRepository bookingRepository,
    ITherapistRepository therapistRepository,
    ITreatmentRepository treatmentRepository) : IChangeTherapistHandler
{
    async Task IChangeTherapistHandler.Handle(ChangeTherapistCommand command)
    {
        var booking = await bookingRepository.GetByIdAsync(command.BookingId)
            ?? throw new NotFoundException("Booking could not be found.");

        var therapist = await therapistRepository.GetByIdAsync(command.TherapistId)
            ?? throw new NotFoundException("Therapist could not be found.");

        var treatment = await treatmentRepository.GetByIdAsync(booking.TreatmentId)
            ?? throw new NotFoundException("Treatment could not be found.");

        if (therapist.CertificationTypes.Contains(treatment.CertificationRequired) == false)
            throw new Exceptions.ApplicationException("Therapist is not qualified for this treatment.");

        var therapistBookings = await bookingRepository.GetAllBookingsByIdAsync(therapist.Id);


        booking.ChangeTherapist(therapist.Id, therapistBookings);

        await bookingRepository.SaveAsync();
    }
}