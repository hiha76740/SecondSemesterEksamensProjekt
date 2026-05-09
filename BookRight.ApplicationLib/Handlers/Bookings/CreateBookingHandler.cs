using BookRight.ApplicationLib.Repositories;
using BookRight.DomainLib.Entities.Bookings;
using BookRight.DomainLib.Exceptions;
using BookRight.DomainLib.Services;
using BookRight.DomainLib.ValueObjects;
using BookRight.FacadeLib.Commands.Booking.DTOs;
using BookRight.FacadeLib.Commands.Booking.Interfaces;

namespace BookRight.ApplicationLib.Handlers.Bookings;

public class CreateBookingHandler(
    IBookingRepository bookingRepository,
    ICustomerRepository customerRepository,
    ITherapistRepository therapistRepository,
    IClinicRepository clinicRepository,
    ITreatmentRepository treatmentRepository,
    IBookingCapacityService bookingCapacityService) : ICreateBookingHandler
{
    async Task ICreateBookingHandler.Handle(CreateBookingCommand command)
    {
        if (command.TreatmentId == Guid.Empty)
            throw new Exceptions.ApplicationException("TreatmentId cannot be empty.");

        if (command.TherapistId == Guid.Empty)
            throw new Exceptions.ApplicationException("TherapistId cannot be empty.");

        if (command.ClinicId == Guid.Empty)
            throw new Exceptions.ApplicationException("ClinicId cannot be empty.");



        var therapist = await therapistRepository.GetByIdAsync(command.TherapistId)
            ?? throw new NotFoundException("Therapist could not be found.");

        var treatment = await treatmentRepository.GetByIdAsync(command.TreatmentId)
            ?? throw new NotFoundException("Treatment could not be found.");

        if (therapist.CertificationTypes.Contains(treatment.CertificationRequired) == false)
            throw new Exceptions.ApplicationException("Therapist is not qualified for this treatment.");


        IReadOnlyList<Booking>? customerBookings = null;

        if (command.CustomerId.HasValue == true)
        {
            _ = await customerRepository.GetByIdAsync(command.CustomerId.Value)
            ?? throw new NotFoundException("Customer could not be found.");

            customerBookings = await bookingRepository.GetAllBookingsByIdAsync(command.CustomerId.Value);
        }
        

        var clinic = await clinicRepository.GetByIdAsync(command.ClinicId)
            ?? throw new NotFoundException("Clinic could not be found.");

        var time = new TimeSlot(command.From, command.To);

        

        var therapistBookings = await bookingRepository.GetAllBookingsByIdAsync(therapist.Id);

        var clinicBookings = await bookingRepository.GetAllBookingsByIdAsync(clinic.Id);

        if (bookingCapacityService.CanCreateBooking(clinic, clinicBookings, time) == false)
            throw new Exceptions.ApplicationException("Clinic capacity was exceeded.");

        var booking = Booking.Create(
            time,
            treatment.Id,
            therapist.Id,
            clinic.Id,
            treatment.Price,
            therapistBookings,
            treatment.MaxParticipants,
            command.CustomerId,
            customerBookings);

        await bookingRepository.AddAsync(booking);

        await bookingRepository.SaveAsync();
    }
}