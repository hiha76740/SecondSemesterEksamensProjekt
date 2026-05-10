using BookRight.ApplicationLib.Repositories;
using BookRight.DomainLib.Exceptions;
using BookRight.FacadeLib.Commands.Booking.DTOs;
using BookRight.FacadeLib.Commands.Booking.Interfaces;

namespace BookRight.ApplicationLib.Handlers.Bookings;

public class ChangeTreatmentHandler(
    IBookingRepository bookingRepository,
    ITreatmentRepository treatmentRepository,
    ITherapistRepository therapistRepository) : IChangeTreatmentHandler
{
    async Task IChangeTreatmentHandler.Handle(ChangeTreatmentCommand command)
    {
        var booking = await bookingRepository.GetByIdAsync(command.BookingId)
            ?? throw new NotFoundException("Booking could not be found.");

        var treatment = await treatmentRepository.GetByIdAsync(command.TreatmentId)
            ?? throw new NotFoundException("Treatment could not be found.");

        var therapist = await therapistRepository.GetByIdAsync(booking.TherapistId)
            ?? throw new NotFoundException("Therapist could not be found.");

        if (therapist.CertificationTypes.Contains(treatment.CertificationRequired) == false)
            throw new Exceptions.ApplicationException("Therapist is not qualified for this treatment.");

        // TODO: Tilføj Price Calculator så bookingen har den korrekte pris ud fra den nye treatment.
        // TODO: Hvad med hvis varigheden ændre sig ?

        booking.ChangeTreatment(treatment.Id);

        await bookingRepository.SaveAsync();
    }
}