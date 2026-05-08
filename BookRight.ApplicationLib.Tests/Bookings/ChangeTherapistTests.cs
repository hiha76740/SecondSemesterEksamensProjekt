using BookRight.ApplicationLib.Handlers.Booking;
using BookRight.ApplicationLib.Repositories;
using BookRight.DomainLib.Entities.Bookings;
using BookRight.DomainLib.Entities.Therapists;
using BookRight.DomainLib.Entities.Treatments.Physiotherapy;
using BookRight.DomainLib.Enums;
using BookRight.DomainLib.Exceptions;
using BookRight.DomainLib.ValueObjects;
using BookRight.FacadeLib.Commands.Booking.DTOs;
using BookRight.FacadeLib.Commands.Booking.Interfaces;
using Moq;

namespace BookRight.ApplicationLib.Tests.Bookings;

public class ChangeTherapistTests
{
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
            Guid.Parse("4504e34a-67a5-4cba-b029-8eb0b453b80d"),
            Guid.Parse("4504e34a-67a5-4cba-b029-8eb0b693b80d"),
            Guid.Parse("4504e34a-67a5-4cba-b029-8eb0b993b80d"),
            Guid.Parse("4504e34a-67a5-4cba-b029-8eb0b493c80d"),
            550m,
            Array.Empty<Booking>(),
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
    // 1. Handle tests (Changing therapist through Application)
    // ---------------------------------------------------------

    [Fact]
    public async Task Handle_GivenValidBookingId_CallsSave()
    {
        // Arrange
        Booking booking = CreateBooking();
        Therapist therapist = CreateTherapist(CertificationTypes.Physiotherapy);
        var treatment = new Physiotherapy30();

        var bookingRepositoryMock = new Mock<IBookingRepository>();
        var therapistRepositoryMock = new Mock<ITherapistRepository>();
        var treatmentRepositoryMock = new Mock<ITreatmentRepository>();

        bookingRepositoryMock
            .Setup(repository => repository.GetByIdAsync(booking.Id))
            .ReturnsAsync(booking);

        therapistRepositoryMock
            .Setup(repository => repository.GetByIdAsync(therapist.Id))
            .ReturnsAsync(therapist);

        treatmentRepositoryMock
            .Setup(repository => repository.GetByIdAsync(booking.TreatmentId))
            .ReturnsAsync(treatment);

        bookingRepositoryMock
            .Setup(repository => repository.GetAllBookingsByIdAsync(therapist.Id))
            .ReturnsAsync(new List<Booking>());

        IChangeTherapistHandler handler = new ChangeTherapistHandler(
            bookingRepositoryMock.Object,
            therapistRepositoryMock.Object,
            treatmentRepositoryMock.Object);

        var command = new ChangeTherapistCommand(
            booking.Id,
            therapist.Id);

        // Act
        await handler.Handle(command);

        // Assert
        bookingRepositoryMock.Verify(repository => repository.SaveAsync(), Times.Once);
    }

    [Fact]
    public async Task Handle_GivenEmptyBookingId_CastApplicationException()
    {
        // Arrange
        var bookingRepositoryMock = new Mock<IBookingRepository>();
        var therapistRepositoryMock = new Mock<ITherapistRepository>();
        var treatmentRepositoryMock = new Mock<ITreatmentRepository>();

        IChangeTherapistHandler handler = new ChangeTherapistHandler(
            bookingRepositoryMock.Object,
            therapistRepositoryMock.Object,
            treatmentRepositoryMock.Object);

        var command = new ChangeTherapistCommand(
            Guid.Empty,
            Guid.NewGuid());

        // Act & Assert
        await Assert.ThrowsAsync<Exceptions.ApplicationException>(() => handler.Handle(command));
    }

    [Fact]
    public async Task Handle_GivenEmptyTherapistId_CastApplicationException()
    {
        // Arrange
        var bookingRepositoryMock = new Mock<IBookingRepository>();
        var therapistRepositoryMock = new Mock<ITherapistRepository>();
        var treatmentRepositoryMock = new Mock<ITreatmentRepository>();

        IChangeTherapistHandler handler = new ChangeTherapistHandler(
            bookingRepositoryMock.Object,
            therapistRepositoryMock.Object,
            treatmentRepositoryMock.Object);

        var command = new ChangeTherapistCommand(
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
        var therapistRepositoryMock = new Mock<ITherapistRepository>();
        var treatmentRepositoryMock = new Mock<ITreatmentRepository>();

        bookingRepositoryMock
            .Setup(repository => repository.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Booking?)null);

        IChangeTherapistHandler handler = new ChangeTherapistHandler(
            bookingRepositoryMock.Object,
            therapistRepositoryMock.Object,
            treatmentRepositoryMock.Object);

        var command = new ChangeTherapistCommand(
            Guid.NewGuid(),
            Guid.NewGuid());

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(command));
    }

    [Fact]
    public async Task Handle_GivenUnknownTherapistId_CastNotFoundException()
    {
        // Arrange
        Booking booking = CreateBooking();

        var bookingRepositoryMock = new Mock<IBookingRepository>();
        var therapistRepositoryMock = new Mock<ITherapistRepository>();
        var treatmentRepositoryMock = new Mock<ITreatmentRepository>();

        bookingRepositoryMock
            .Setup(repository => repository.GetByIdAsync(booking.Id))
            .ReturnsAsync(booking);

        therapistRepositoryMock
            .Setup(repository => repository.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Therapist?)null);

        IChangeTherapistHandler handler = new ChangeTherapistHandler(
            bookingRepositoryMock.Object,
            therapistRepositoryMock.Object,
            treatmentRepositoryMock.Object);

        var command = new ChangeTherapistCommand(
            booking.Id,
            Guid.NewGuid());

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(command));
    }

    [Fact]
    public async Task Handle_GivenTherapistWithoutRequiredCertification_CastApplicationException()
    {
        // Arrange
        Booking booking = CreateBooking();
        Therapist therapist = CreateTherapist(CertificationTypes.Acupuncture);
        var treatment = new Physiotherapy30();

        var bookingRepositoryMock = new Mock<IBookingRepository>();
        var therapistRepositoryMock = new Mock<ITherapistRepository>();
        var treatmentRepositoryMock = new Mock<ITreatmentRepository>();

        bookingRepositoryMock
            .Setup(repository => repository.GetByIdAsync(booking.Id))
            .ReturnsAsync(booking);

        therapistRepositoryMock
            .Setup(repository => repository.GetByIdAsync(therapist.Id))
            .ReturnsAsync(therapist);

        treatmentRepositoryMock
            .Setup(repository => repository.GetByIdAsync(booking.TreatmentId))
            .ReturnsAsync(treatment);

        IChangeTherapistHandler handler = new ChangeTherapistHandler(
            bookingRepositoryMock.Object,
            therapistRepositoryMock.Object,
            treatmentRepositoryMock.Object);

        var command = new ChangeTherapistCommand(
            booking.Id,
            therapist.Id);

        // Act & Assert
        await Assert.ThrowsAsync<Exceptions.ApplicationException>(() => handler.Handle(command));
    }
}