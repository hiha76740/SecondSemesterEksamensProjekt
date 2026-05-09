using BookRight.ApplicationLib.Handlers.Bookings;
using BookRight.ApplicationLib.Repositories;
using BookRight.DomainLib.Entities.Bookings;
using BookRight.DomainLib.Entities.Clinics;
using BookRight.DomainLib.Entities.Customers;
using BookRight.DomainLib.Entities.Therapists;
using BookRight.DomainLib.Entities.Treatments.Physiotherapy;
using BookRight.DomainLib.Enums;
using BookRight.DomainLib.Exceptions;
using BookRight.DomainLib.Services;
using BookRight.DomainLib.ValueObjects;
using BookRight.FacadeLib.Commands.Booking.DTOs;
using BookRight.FacadeLib.Commands.Booking.Interfaces;
using Moq;

namespace BookRight.ApplicationLib.Tests.Bookings;

public class CreateBookingTests
{
    private static TimeSlot CreateTimeSlot(int fromHour, int toHour)
    {
        return new TimeSlot(
            DateTime.UtcNow.AddDays(1).Date.AddHours(fromHour),
            DateTime.UtcNow.AddDays(1).Date.AddHours(toHour));
    }

    private static Customer CreateCustomer()
    {
        return Customer.Create(
            "Test",
            "Customer",
            new DateTime(1995, 1, 1),
            new Address("Testvej 1", "7100", "Vejle"),
            new Email("test@test.dk"),
            new PhoneNumber("12345678"));
    }

    private static Therapist CreateTherapist(CertificationTypes certificationType)
    {
        return Therapist.Create(
            "AUTH123",
            "Test Therapist",
            500m,
            new Address("Testvej 2", "7100", "Vejle"),
            new Email("therapist@test.dk"),
            new PhoneNumber("87654321"),
            new List<Guid> { Guid.Parse("4504e34a-67a5-4cba-b029-8eb0b493c80d") },
            new List<CertificationTypes> { certificationType });
    }

    private static Clinic CreateClinic()
    {
        return Clinic.Create(
            "Klinik Vejle",
            5,
            new OpeningHours(
                DateTime.Now.AddDays(1).Date.AddHours(8),
                DateTime.Now.AddDays(1).Date.AddHours(16)),
            new Address("Testvej 1", "7100", "Vejle"));
    }


    // ---------------------------------------------------------
    // 1. Handle tests (Creating a Booking through Application)
    // ---------------------------------------------------------

    [Fact]
    public async Task Handle_GivenValidData_CallsAddAndSave()
    {
        // Arrange
        Customer customer = CreateCustomer();
        Therapist therapist = CreateTherapist(CertificationTypes.Physiotherapy);
        Clinic clinic = CreateClinic();
        var treatment = new Physiotherapy30();

        var mockCustomerRepo = new Mock<ICustomerRepository>();
        var mockTherapistRepo = new Mock<ITherapistRepository>();
        var mockClinicRepo = new Mock<IClinicRepository>();
        var mockTreatmentRepo = new Mock<ITreatmentRepository>();
        var mockBookingRepo = new Mock<IBookingRepository>();
        var mockBookingCapacityService = new Mock<IBookingCapacityService>();

        mockCustomerRepo
                .Setup(r => r.GetByIdAsync(customer.Id))
                .ReturnsAsync(customer);

        mockTherapistRepo
            .Setup(r => r.GetByIdAsync(therapist.Id))
            .ReturnsAsync(therapist);

        mockClinicRepo
            .Setup(r => r.GetByIdAsync(clinic.Id))
            .ReturnsAsync(clinic);

        mockTreatmentRepo
            .Setup(r => r.GetByIdAsync(treatment.Id))
            .ReturnsAsync(treatment);

        mockBookingRepo
            .Setup(r => r.GetAllBookingsByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(new List<Booking>());

        mockBookingCapacityService
            .Setup(s => s.CanCreateBooking(clinic, It.IsAny<IEnumerable<Booking>>(), It.IsAny<TimeSlot>()))
            .Returns(true);

        TimeSlot timeSlot = CreateTimeSlot(9, 10);

        var command = new CreateBookingCommand(
            customer.Id,
            treatment.Id,
            therapist.Id,
            clinic.Id,
            timeSlot.From,
            timeSlot.To);

        var handler = new CreateBookingHandler(mockBookingRepo.Object, mockCustomerRepo.Object,mockTherapistRepo.Object,mockClinicRepo.Object,mockTreatmentRepo.Object,mockBookingCapacityService.Object) as ICreateBookingHandler;

        // Act
        await handler.Handle(command);

        // Assert
        mockBookingRepo.Verify(r => r.AddAsync(It.IsAny<Booking>()), Times.Once);
        mockBookingRepo.Verify(r => r.SaveAsync(), Times.Once);
    }

    [Fact]
    public async Task Handle_GivenEmptyCustomerId_CastApplicationException()
    {
        // Arrange
        var mockCustomerRepo = new Mock<ICustomerRepository>();
        var mockTherapistRepo = new Mock<ITherapistRepository>();
        var mockClinicRepo = new Mock<IClinicRepository>();
        var mockTreatmentRepo = new Mock<ITreatmentRepository>();
        var mockBookingRepo = new Mock<IBookingRepository>();
        var mockBookingCapacityService = new Mock<IBookingCapacityService>();

        TimeSlot timeSlot = CreateTimeSlot(9, 10);

        var command = new CreateBookingCommand(
            Guid.Empty,
            Guid.NewGuid(),
            Guid.NewGuid(),
            Guid.NewGuid(),
            timeSlot.From,
            timeSlot.To);

        var handler = new CreateBookingHandler(mockBookingRepo.Object, mockCustomerRepo.Object,mockTherapistRepo.Object,mockClinicRepo.Object,mockTreatmentRepo.Object,mockBookingCapacityService.Object) as ICreateBookingHandler;

        // Act & Assert
        await Assert.ThrowsAsync<Exceptions.ApplicationException>(() => handler.Handle(command));
    }

    [Fact]
    public async Task Handle_GivenUnknownCustomerId_CastNotFoundException()
    {
        // Arrange
        var mockCustomerRepo = new Mock<ICustomerRepository>();
        var mockTherapistRepo = new Mock<ITherapistRepository>();
        var mockClinicRepo = new Mock<IClinicRepository>();
        var mockTreatmentRepo = new Mock<ITreatmentRepository>();
        var mockBookingRepo = new Mock<IBookingRepository>();
        var mockBookingCapacityService = new Mock<IBookingCapacityService>();

        mockCustomerRepo
            .Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Customer?)null);

        TimeSlot timeSlot = CreateTimeSlot(9, 10);

        var command = new CreateBookingCommand(
            Guid.NewGuid(),
            Guid.NewGuid(),
            Guid.NewGuid(),
            Guid.NewGuid(),
            timeSlot.From,
            timeSlot.To);

        var handler = new CreateBookingHandler(mockBookingRepo.Object, mockCustomerRepo.Object, mockTherapistRepo.Object, mockClinicRepo.Object, mockTreatmentRepo.Object, mockBookingCapacityService.Object) as ICreateBookingHandler;


        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(command));
    }

    [Fact]
    public async Task Handle_GivenTherapistWithoutRequiredCertification_CastApplicationException()
    {
        // Arrange
        Customer customer = CreateCustomer();
        Therapist therapist = CreateTherapist(CertificationTypes.Acupuncture);
        Clinic clinic = CreateClinic();
        var treatment = new Physiotherapy30();

        var mockCustomerRepo = new Mock<ICustomerRepository>();
        var mockTherapistRepo = new Mock<ITherapistRepository>();
        var mockClinicRepo = new Mock<IClinicRepository>();
        var mockTreatmentRepo = new Mock<ITreatmentRepository>();
        var mockBookingRepo = new Mock<IBookingRepository>();
        var mockBookingCapacityService = new Mock<IBookingCapacityService>();

        mockCustomerRepo
            .Setup(r => r.GetByIdAsync(customer.Id))
            .ReturnsAsync(customer);

        mockTherapistRepo
            .Setup(r => r.GetByIdAsync(therapist.Id))
            .ReturnsAsync(therapist);

        mockClinicRepo
            .Setup(r => r.GetByIdAsync(clinic.Id))
            .ReturnsAsync(clinic);

        mockTreatmentRepo
            .Setup(r => r.GetByIdAsync(treatment.Id))
            .ReturnsAsync(treatment);

        TimeSlot timeSlot = CreateTimeSlot(9, 10);

        var command = new CreateBookingCommand(
            customer.Id,
            treatment.Id,
            therapist.Id,
            clinic.Id,
            timeSlot.From,
            timeSlot.To);

        var handler = new CreateBookingHandler(mockBookingRepo.Object, mockCustomerRepo.Object, mockTherapistRepo.Object, mockClinicRepo.Object, mockTreatmentRepo.Object, mockBookingCapacityService.Object) as ICreateBookingHandler;


        // Act & Assert
        await Assert.ThrowsAsync<Exceptions.ApplicationException>(() => handler.Handle(command));
    }

    [Fact]
    public async Task Handle_GivenNoClinicCapacity_CastApplicationException()
    {
        // Arrange
        Customer customer = CreateCustomer();
        Therapist therapist = CreateTherapist(CertificationTypes.Physiotherapy);
        Clinic clinic = CreateClinic();
        var treatment = new Physiotherapy30();

        var mockCustomerRepo = new Mock<ICustomerRepository>();
        var mockTherapistRepo = new Mock<ITherapistRepository>();
        var mockClinicRepo = new Mock<IClinicRepository>();
        var mockTreatmentRepo = new Mock<ITreatmentRepository>();
        var mockBookingRepo = new Mock<IBookingRepository>();
        var mockBookingCapacityService = new Mock<IBookingCapacityService>();

        mockCustomerRepo
            .Setup(r => r.GetByIdAsync(customer.Id))
            .ReturnsAsync(customer);

        mockTherapistRepo
            .Setup(r => r.GetByIdAsync(therapist.Id))
            .ReturnsAsync(therapist);

        mockClinicRepo
            .Setup(r => r.GetByIdAsync(clinic.Id))
            .ReturnsAsync(clinic);

        mockTreatmentRepo
            .Setup(r => r.GetByIdAsync(treatment.Id))
            .ReturnsAsync(treatment);

        mockBookingRepo
            .Setup(r => r.GetAllBookingsByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(new List<Booking>());

        mockBookingCapacityService
            .Setup(s => s.CanCreateBooking(clinic, It.IsAny<IEnumerable<Booking>>(), It.IsAny<TimeSlot>()))
            .Returns(false);

        TimeSlot timeSlot = CreateTimeSlot(9, 10);

        var command = new CreateBookingCommand(
            customer.Id,
            treatment.Id,
            therapist.Id,
            clinic.Id,
            timeSlot.From,
            timeSlot.To);

        var handler = new CreateBookingHandler(mockBookingRepo.Object, mockCustomerRepo.Object, mockTherapistRepo.Object, mockClinicRepo.Object, mockTreatmentRepo.Object, mockBookingCapacityService.Object) as ICreateBookingHandler;


        // Act & Assert
        await Assert.ThrowsAsync<Exceptions.ApplicationException>(() => handler.Handle(command));
    }
}