using BookRight.ApplicationLib.Repositories;
using BookRight.DomainLib.Exceptions;
using BookRight.DomainLib.Services;
using BookRight.DomainLib.ValueObjects;
using BookRight.FacadeLib.Commands.Booking.DTOs;
using BookRight.FacadeLib.Commands.Booking.Interfaces;

namespace BookRight.ApplicationLib.Handlers.Booking;

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
        if (command.CustomerId == Guid.Empty)
            throw new Exceptions.ApplicationException("CustomerId cannot be empty.");

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

        var customer = await customerRepository.GetByIdAsync(command.CustomerId)
            ?? throw new NotFoundException("Customer could not be found.");

        var clinic = await clinicRepository.GetByIdAsync(command.ClinicId)
            ?? throw new NotFoundException("Clinic could not be found.");



        var timeSlot = new TimeSlot(command.From, command.To);

        var customerBookings = await bookingRepository.GetAllBookingsByIdAsync(customer.Id);

        var therapistBookings = await bookingRepository.GetAllBookingsByIdAsync(therapist.Id);

        var clinicBookings = await bookingRepository.GetAllBookingsByIdAsync(clinic.Id);

        if (bookingCapacityService.CanCreateBooking(clinic, clinicBookings, timeSlot) == false)
            throw new Exceptions.ApplicationException("Clinic capacity was exceeded.");

        var booking = BookRight.DomainLib.Entities.Bookings.Booking.Create(
            timeSlot,
            customer.Id,
            treatment.Id,
            therapist.Id,
            clinic.Id,
            treatment.Price,
            customerBookings,
            therapistBookings);

        await bookingRepository.AddAsync(booking);

        await bookingRepository.SaveAsync();
    }
}