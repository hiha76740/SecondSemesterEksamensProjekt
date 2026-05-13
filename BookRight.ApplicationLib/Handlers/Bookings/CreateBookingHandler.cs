using BookRight.ApplicationLib.Repositories;
using BookRight.DomainLib.Entities.Bookings;
using BookRight.DomainLib.Enums;
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
    IBookingCapacityService bookingCapacityService,
    IValidateOverlapService overlapService) : ICreateBookingHandler
{
    async Task ICreateBookingHandler.Handle(CreateBookingCommand command)
    {
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

            customerBookings = await bookingRepository.GetBookingsByCustomerIdAsync(command.CustomerId.Value);
        }
        

        var clinic = await clinicRepository.GetByIdAsync(command.ClinicId)
            ?? throw new NotFoundException("Clinic could not be found.");

        var time = new TimeSlot(command.From, command.To);

        bool exsists = Enum.TryParse<DiscountTypes>(command.DiscountTypeUsed, out var discountTypeUsed);

        if (exsists == false)
            throw new NotFoundException("Discount type was not found");

        var therapistBookings = await bookingRepository.GetBookingsByTherapistIdAsync(therapist.Id);

        var clinicBookings = await bookingRepository.GetBookingsByClinicIdAsync(clinic.Id);

        if (bookingCapacityService.CanCreateBooking(clinic, clinicBookings, time) == false)
            throw new Exceptions.ApplicationException("Clinic capacity was exceeded.");

        var booking = Booking.Create(
            time,
            treatment.Id,
            therapist.Id,
            clinic.Id,
            treatment.Price,
            treatment.MaxParticipants,
            discountTypeUsed,
            command.CustomerId);

        overlapService.Validate(booking, customerBookings!);
        overlapService.Validate(booking, therapistBookings);

        await bookingRepository.AddAsync(booking);

        await bookingRepository.SaveAsync();
    }
}