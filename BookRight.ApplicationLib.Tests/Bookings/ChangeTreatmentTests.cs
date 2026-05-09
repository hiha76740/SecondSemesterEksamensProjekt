using BookRight.ApplicationLib.Handlers.Bookings;
using BookRight.ApplicationLib.Repositories;
using BookRight.DomainLib.Entities.Bookings;
using BookRight.DomainLib.Entities.Therapists;
using BookRight.DomainLib.Entities.Treatments;
using BookRight.DomainLib.Enums;
using BookRight.DomainLib.Exceptions;
using BookRight.DomainLib.ValueObjects;
using BookRight.FacadeLib.Commands.Booking.DTOs;
using BookRight.FacadeLib.Commands.Booking.Interfaces;
using Moq;

namespace BookRight.ApplicationLib.Tests.Bookings;

public class ChangeTreatmentTests
{
    private static Treatment CreateTreatment()
    {
        return Treatment.Create("Physiotherapy", 395, TimeSpan.FromMinutes(30), CertificationTypes.Physiotherapy, 1);
    }

    private static TimeSlot CreateTimeSlot(int fromHour, int toHour)
    {
        return new TimeSlot(
            DateTime.UtcNow.AddDays(1).Date.AddHours(fromHour),
            DateTime.UtcNow.AddDays(1).Date.AddHours(toHour));
    }

    private static Booking CreateBooking()
    {
        return Booking.Create(
            CreateTimeSlot(9, 10),
            Guid.Parse("4504e34a-67a5-4cba-b029-8eb0b693b80d"),
            Guid.Parse("4504e34a-67a5-4cba-b029-8eb0b993b80d"),
            Guid.Parse("4504e34a-67a5-4cba-b029-8eb0b493c80d"),
            550m,
            Array.Empty<Booking>(),
            1,
            Guid.Parse("4504e34a-67a5-4cba-b029-8eb0b453b80d"),
            Array.Empty<Booking>());
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

    // ---------------------------------------------------------
    // 1. Handle tests (Changing treatment through Application)
    // ---------------------------------------------------------

    [Fact]
    public async Task Handle_GivenValidBookingId_CallsSave()
    {
        // Arrange
        var booking = CreateBooking();
        var therapist = CreateTherapist(CertificationTypes.Physiotherapy);
        var treatment = CreateTreatment();

        var bookingRepositoryMock = new Mock<IBookingRepository>();
        var treatmentRepositoryMock = new Mock<ITreatmentRepository>();
        var therapistRepositoryMock = new Mock<ITherapistRepository>();

        bookingRepositoryMock
            .Setup(r => r.GetByIdAsync(booking.Id))
            .ReturnsAsync(booking);

        treatmentRepositoryMock
            .Setup(r => r.GetByIdAsync(treatment.Id))
            .ReturnsAsync(treatment);

        therapistRepositoryMock
            .Setup(r => r.GetByIdAsync(booking.TherapistId))
            .ReturnsAsync(therapist);

        IChangeTreatmentHandler handler = new ChangeTreatmentHandler(
            bookingRepositoryMock.Object,
            treatmentRepositoryMock.Object,
            therapistRepositoryMock.Object);

        var command = new ChangeTreatmentCommand(
            booking.Id,
            treatment.Id);

        // Act
        await handler.Handle(command);

        // Assert
        bookingRepositoryMock.Verify(r => r.SaveAsync(), Times.Once);
    }

    [Fact]
    public async Task Handle_GivenEmptyBookingId_CastApplicationException()
    {
        // Arrange
        var bookingRepositoryMock = new Mock<IBookingRepository>();
        var treatmentRepositoryMock = new Mock<ITreatmentRepository>();
        var therapistRepositoryMock = new Mock<ITherapistRepository>();

        IChangeTreatmentHandler handler = new ChangeTreatmentHandler(
            bookingRepositoryMock.Object,
            treatmentRepositoryMock.Object,
            therapistRepositoryMock.Object);

        var command = new ChangeTreatmentCommand(
            Guid.Empty,
            Guid.NewGuid());

        // Act & Assert
        await Assert.ThrowsAsync<Exceptions.ApplicationException>(() => handler.Handle(command));
    }

    [Fact]
    public async Task Handle_GivenEmptyTreatmentId_CastApplicationException()
    {
        // Arrange
        var bookingRepositoryMock = new Mock<IBookingRepository>();
        var treatmentRepositoryMock = new Mock<ITreatmentRepository>();
        var therapistRepositoryMock = new Mock<ITherapistRepository>();

        IChangeTreatmentHandler handler = new ChangeTreatmentHandler(
            bookingRepositoryMock.Object,
            treatmentRepositoryMock.Object,
            therapistRepositoryMock.Object);

        var command = new ChangeTreatmentCommand(
            Guid.NewGuid(),
            Guid.Empty);

        // Act & Assert
        await Assert.ThrowsAsync<Exceptions.ApplicationException>(() => handler.Handle(command));
    }

    [Fact]
    public async Task Handle_GivenUnknownBookingId_CastNotFoundException()
    {
        // Arrange
        var bookingRepositoryMock = new Mock<IBookingRepository>();
        var treatmentRepositoryMock = new Mock<ITreatmentRepository>();
        var therapistRepositoryMock = new Mock<ITherapistRepository>();

        bookingRepositoryMock
            .Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Booking?)null);

        IChangeTreatmentHandler handler = new ChangeTreatmentHandler(
            bookingRepositoryMock.Object,
            treatmentRepositoryMock.Object,
            therapistRepositoryMock.Object);

        var command = new ChangeTreatmentCommand(
            Guid.NewGuid(),
            Guid.NewGuid());

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(command));
    }

    [Fact]
    public async Task Handle_GivenUnknownTreatmentId_CastNotFoundException()
    {
        // Arrange
        Booking booking = CreateBooking();

        var bookingRepositoryMock = new Mock<IBookingRepository>();
        var treatmentRepositoryMock = new Mock<ITreatmentRepository>();
        var therapistRepositoryMock = new Mock<ITherapistRepository>();

        bookingRepositoryMock
            .Setup(r => r.GetByIdAsync(booking.Id))
            .ReturnsAsync(booking);

        treatmentRepositoryMock
            .Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Treatment?)null);

        IChangeTreatmentHandler handler = new ChangeTreatmentHandler(
            bookingRepositoryMock.Object,
            treatmentRepositoryMock.Object,
            therapistRepositoryMock.Object);

        var command = new ChangeTreatmentCommand(
            booking.Id,
            Guid.NewGuid());

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(command));
    }

    [Fact]
    public async Task Handle_GivenUnknownTherapistId_CastNotFoundException()
    {
        // Arrange
        var booking = CreateBooking();
        var treatment = CreateTreatment();

        var bookingRepositoryMock = new Mock<IBookingRepository>();
        var treatmentRepositoryMock = new Mock<ITreatmentRepository>();
        var therapistRepositoryMock = new Mock<ITherapistRepository>();

        bookingRepositoryMock
            .Setup(r => r.GetByIdAsync(booking.Id))
            .ReturnsAsync(booking);

        treatmentRepositoryMock
            .Setup(r => r.GetByIdAsync(treatment.Id))
            .ReturnsAsync(treatment);

        therapistRepositoryMock
            .Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Therapist?)null);

        IChangeTreatmentHandler handler = new ChangeTreatmentHandler(
            bookingRepositoryMock.Object,
            treatmentRepositoryMock.Object,
            therapistRepositoryMock.Object);

        var command = new ChangeTreatmentCommand(
            booking.Id,
            treatment.Id);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(command));
    }

    [Fact]
    public async Task Handle_GivenTherapistWithoutRequiredCertification_CastApplicationException()
    {
        // Arrange
        var booking = CreateBooking();
        var therapist = CreateTherapist(CertificationTypes.Acupuncture);
        var treatment = CreateTreatment();

        var bookingRepositoryMock = new Mock<IBookingRepository>();
        var treatmentRepositoryMock = new Mock<ITreatmentRepository>();
        var therapistRepositoryMock = new Mock<ITherapistRepository>();

        bookingRepositoryMock
            .Setup(r => r.GetByIdAsync(booking.Id))
            .ReturnsAsync(booking);

        treatmentRepositoryMock
            .Setup(r => r.GetByIdAsync(treatment.Id))
            .ReturnsAsync(treatment);

        therapistRepositoryMock
            .Setup(r => r.GetByIdAsync(booking.TherapistId))
            .ReturnsAsync(therapist);

        IChangeTreatmentHandler handler = new ChangeTreatmentHandler(
            bookingRepositoryMock.Object,
            treatmentRepositoryMock.Object,
            therapistRepositoryMock.Object);

        var command = new ChangeTreatmentCommand(
            booking.Id,
            treatment.Id);

        // Act & Assert
        await Assert.ThrowsAsync<Exceptions.ApplicationException>(() => handler.Handle(command));
    }
}