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
    private readonly Mock<IBookingRepository> _mockBookingRepo = new();
    private readonly Mock<ICustomerRepository> _mockCustomerRepo = new();
    private readonly Mock<ITherapistRepository> _mockTherapistRepo = new();
    private readonly Mock<IClinicRepository> _mockClinicRepo = new();
    private readonly Mock<ITreatmentRepository> _mockTreatmentRepo = new();
    private readonly Mock<IBookingCapacityService> _mockBookingCapacityService = new();

    private ICreateBookingHandler CreateSut() => new CreateBookingHandler(
        _mockBookingRepo.Object,
        _mockCustomerRepo.Object,
        _mockTherapistRepo.Object,
        _mockClinicRepo.Object,
        _mockTreatmentRepo.Object,
        _mockBookingCapacityService.Object);


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

        _mockCustomerRepo
            .Setup(repository => repository.GetByIdAsync(customer.Id))
            .ReturnsAsync(customer);

        _mockTherapistRepo
            .Setup(repository => repository.GetByIdAsync(therapist.Id))
            .ReturnsAsync(therapist);

        _mockClinicRepo
            .Setup(repository => repository.GetByIdAsync(clinic.Id))
            .ReturnsAsync(clinic);

        _mockTreatmentRepo
            .Setup(repository => repository.GetByIdAsync(treatment.Id))
            .ReturnsAsync(treatment);

        _mockBookingRepo
            .Setup(repository => repository.GetAllBookingsByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(new List<Booking>());

        _mockBookingCapacityService
            .Setup(service => service.CanCreateBooking(clinic, It.IsAny<IEnumerable<Booking>>(), It.IsAny<TimeSlot>()))
            .Returns(true);

        TimeSlot timeSlot = CreateTimeSlot(9, 10);

        var command = new CreateBookingCommand(
            customer.Id,
            treatment.Id,
            therapist.Id,
            clinic.Id,
            timeSlot.From,
            timeSlot.To);

        // Act
        await CreateSut().Handle(command);

        // Assert
        _mockBookingRepo.Verify(repository => repository.AddAsync(It.IsAny<Booking>()), Times.Once);
        _mockBookingRepo.Verify(repository => repository.SaveAsync(), Times.Once);
    }

    [Fact]
    public async Task Handle_GivenEmptyCustomerId_CastApplicationException()
    {
        // Arrange
        TimeSlot timeSlot = CreateTimeSlot(9, 10);

        var command = new CreateBookingCommand(
            Guid.Empty,
            Guid.NewGuid(),
            Guid.NewGuid(),
            Guid.NewGuid(),
            timeSlot.From,
            timeSlot.To);

        // Act & Assert
        await Assert.ThrowsAsync<Exceptions.ApplicationException>(() => CreateSut().Handle(command));
    }

    [Fact]
    public async Task Handle_GivenUnknownCustomerId_CastNotFoundException()
    {
        // Arrange
        _mockCustomerRepo
            .Setup(repository => repository.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Customer?)null);

        TimeSlot timeSlot = CreateTimeSlot(9, 10);

        var command = new CreateBookingCommand(
            Guid.NewGuid(),
            Guid.NewGuid(),
            Guid.NewGuid(),
            Guid.NewGuid(),
            timeSlot.From,
            timeSlot.To);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => CreateSut().Handle(command));
    }

    [Fact]
    public async Task Handle_GivenTherapistWithoutRequiredCertification_CastApplicationException()
    {
        // Arrange
        Customer customer = CreateCustomer();
        Therapist therapist = CreateTherapist(CertificationTypes.Acupuncture);
        Clinic clinic = CreateClinic();
        var treatment = new Physiotherapy30();

        _mockCustomerRepo
            .Setup(repository => repository.GetByIdAsync(customer.Id))
            .ReturnsAsync(customer);

        _mockTherapistRepo
            .Setup(repository => repository.GetByIdAsync(therapist.Id))
            .ReturnsAsync(therapist);

        _mockClinicRepo
            .Setup(repository => repository.GetByIdAsync(clinic.Id))
            .ReturnsAsync(clinic);

        _mockTreatmentRepo
            .Setup(repository => repository.GetByIdAsync(treatment.Id))
            .ReturnsAsync(treatment);

        TimeSlot timeSlot = CreateTimeSlot(9, 10);

        var command = new CreateBookingCommand(
            customer.Id,
            treatment.Id,
            therapist.Id,
            clinic.Id,
            timeSlot.From,
            timeSlot.To);

        // Act & Assert
        await Assert.ThrowsAsync<Exceptions.ApplicationException>(() => CreateSut().Handle(command));
    }

    [Fact]
    public async Task Handle_GivenNoClinicCapacity_CastApplicationException()
    {
        // Arrange
        Customer customer = CreateCustomer();
        Therapist therapist = CreateTherapist(CertificationTypes.Physiotherapy);
        Clinic clinic = CreateClinic();
        var treatment = new Physiotherapy30();

        _mockCustomerRepo
            .Setup(repository => repository.GetByIdAsync(customer.Id))
            .ReturnsAsync(customer);

        _mockTherapistRepo
            .Setup(repository => repository.GetByIdAsync(therapist.Id))
            .ReturnsAsync(therapist);

        _mockClinicRepo
            .Setup(repository => repository.GetByIdAsync(clinic.Id))
            .ReturnsAsync(clinic);

        _mockTreatmentRepo
            .Setup(repository => repository.GetByIdAsync(treatment.Id))
            .ReturnsAsync(treatment);

        _mockBookingRepo
            .Setup(repository => repository.GetAllBookingsByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(new List<Booking>());

        _mockBookingCapacityService
            .Setup(service => service.CanCreateBooking(clinic, It.IsAny<IEnumerable<Booking>>(), It.IsAny<TimeSlot>()))
            .Returns(false);

        TimeSlot timeSlot = CreateTimeSlot(9, 10);

        var command = new CreateBookingCommand(
            customer.Id,
            treatment.Id,
            therapist.Id,
            clinic.Id,
            timeSlot.From,
            timeSlot.To);

        // Act & Assert
        await Assert.ThrowsAsync<Exceptions.ApplicationException>(() => CreateSut().Handle(command));
    }
}