using BookRight.ApplicationLib.Exceptions;
using BookRight.ApplicationLib.Handlers.Booking;
using BookRight.ApplicationLib.Repositories;
using BookRight.DomainLib.Entities.Bookings;
using BookRight.DomainLib.Enums;
using BookRight.DomainLib.ValueObjects;
using BookRight.FacadeLib.Booking.DTOs;
using BookRight.FacadeLib.Booking.Interfaces;
using Moq;

namespace BookRight.ApplicationLib.Tests.Handlers.Bookings;

public class CancelBookingTests
{
    private static Guid CustomerId => Guid.Parse("4504e34a-67a5-4cba-b029-8eb0b453b80d");
    private static Guid TreatmentId => Guid.Parse("4504e34a-67a5-4cba-b029-8eb0b693b80d");
    private static Guid TherapistId => Guid.Parse("4504e34a-67a5-4cba-b029-8eb0b993b80d");
    private static Guid ClinicId => Guid.Parse("4504e34a-67a5-4cba-b029-8eb0b493c80d");

    private static Guid UnknownBookingId => Guid.Parse("4504e34a-67a5-4cba-b029-8eb0b493b81d");

    private static decimal Price => 550m;

    private static IEnumerable<Booking> ExistingCustomerBookings => Array.Empty<Booking>();
    private static IEnumerable<Booking> ExistingTherapistBookings => Array.Empty<Booking>();

    private static TimeSlot CreateTimeSlot(int fromHour, int toHour)
    {
        return new TimeSlot(
            DateTime.UtcNow.AddDays(1).Date.AddHours(fromHour),
            DateTime.UtcNow.AddDays(1).Date.AddHours(toHour));
    }

    private static Booking CreateWithoutOverlap(
        TimeSlot? timeSlot = null,
        Guid? customerId = null,
        Guid? treatmentId = null,
        Guid? therapistId = null,
        Guid? clinicId = null,
        decimal? price = null,
        IEnumerable<Booking>? existingCustomerBookings = null,
        IEnumerable<Booking>? existingTherapistBookings = null)
    {
        return Booking.Create(
            timeSlot ?? CreateTimeSlot(9, 10),
            customerId ?? CustomerId,
            treatmentId ?? TreatmentId,
            therapistId ?? TherapistId,
            clinicId ?? ClinicId,
            price ?? Price,
            existingCustomerBookings ?? ExistingCustomerBookings,
            existingTherapistBookings ?? ExistingTherapistBookings);
    }

    // ---------------------------------------------------------
    // 1. Handle tests (Cancelling a Booking through Application)
    // ---------------------------------------------------------

    [Fact]
    public async Task Handle_GivenValidBookingId_SetsStatusToCancelled()
    {
        // Arrange
        Booking booking = CreateWithoutOverlap();

        var bookingRepositoryMock = new Mock<IBookingRepository>();

        bookingRepositoryMock
            .Setup(repository => repository.GetByIdAsync(booking.Id))
            .ReturnsAsync(booking);

        bookingRepositoryMock
            .Setup(repository => repository.Save())
            .Returns(Task.CompletedTask);

        ICancelBookingHandler handler = new CancelBookingHandler(bookingRepositoryMock.Object);

        var command = new CancelBookingCommand(booking.Id);

        // Act
        await handler.Handle(command);

        // Assert
        Assert.Equal(BookingStatus.Cancelled, booking.Status);
    }

    [Fact]
    public async Task Handle_GivenValidBookingId_CallsSave()
    {
        // Arrange
        Booking booking = CreateWithoutOverlap();

        var bookingRepositoryMock = new Mock<IBookingRepository>();

        bookingRepositoryMock
            .Setup(repository => repository.GetByIdAsync(booking.Id))
            .ReturnsAsync(booking);

        bookingRepositoryMock
            .Setup(repository => repository.Save())
            .Returns(Task.CompletedTask);

        ICancelBookingHandler handler = new CancelBookingHandler(bookingRepositoryMock.Object);

        var command = new CancelBookingCommand(booking.Id);

        // Act
        await handler.Handle(command);

        // Assert
        bookingRepositoryMock.Verify(repository => repository.Save(), Times.Once);
    }

    [Fact]
    public async Task Handle_GivenEmptyBookingId_CastApplicationException()
    {
        // Arrange
        var bookingRepositoryMock = new Mock<IBookingRepository>();

        ICancelBookingHandler handler = new CancelBookingHandler(bookingRepositoryMock.Object);

        var command = new CancelBookingCommand(Guid.Empty);

        // Act & Assert
        await Assert.ThrowsAsync<BookRight.ApplicationLib.Exceptions.ApplicationException>(() => handler.Handle(command));

        bookingRepositoryMock.Verify(repository => repository.GetByIdAsync(It.IsAny<Guid>()), Times.Never);

        bookingRepositoryMock.Verify(repository => repository.Save(), Times.Never);
    }

    [Fact]
    public async Task Handle_GivenUnknownBookingId_CastNotFoundException()
    {
        // Arrange
        var bookingRepositoryMock = new Mock<IBookingRepository>();

        bookingRepositoryMock
            .Setup(repository => repository.GetByIdAsync(UnknownBookingId))
            .ReturnsAsync((Booking?)null);

        ICancelBookingHandler handler = new CancelBookingHandler(bookingRepositoryMock.Object);

        var command = new CancelBookingCommand(UnknownBookingId);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(command));

        bookingRepositoryMock.Verify(repository => repository.Save(), Times.Never);
    }
}