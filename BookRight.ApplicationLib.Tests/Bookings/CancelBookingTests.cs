using BookRight.ApplicationLib.Handlers.Bookings;
using BookRight.ApplicationLib.Repositories;
using BookRight.DomainLib.Entities.Bookings;
using BookRight.DomainLib.Exceptions;
using BookRight.DomainLib.ValueObjects;
using BookRight.FacadeLib.Commands.Booking.DTOs;
using BookRight.FacadeLib.Commands.Booking.Interfaces;
using Moq;

namespace BookRight.ApplicationLib.Tests.Bookings;

public class CancelBookingTests
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
            Guid.Parse("4504e34a-67a5-4cba-b029-8eb0b693b80d"),
            Guid.Parse("4504e34a-67a5-4cba-b029-8eb0b993b80d"),
            Guid.Parse("4504e34a-67a5-4cba-b029-8eb0b493c80d"),
            550m,
            1,
            Guid.Parse("4504e34a-67a5-4cba-b029-8eb0b453b80d"));
    }

    // ---------------------------------------------------------
    // 1. Handle tests (Cancelling a Booking through Application)
    // ---------------------------------------------------------

    [Fact]
    public async Task Handle_GivenValidBookingId_CallsSave()
    {
        // Arrange
        Booking booking = CreateBooking();

        var bookingRepositoryMock = new Mock<IBookingRepository>();

        bookingRepositoryMock
            .Setup(r => r.GetByIdAsync(booking.Id))
            .ReturnsAsync(booking);

        ICancelBookingHandler handler = new CancelBookingHandler(bookingRepositoryMock.Object);

        var command = new CancelBookingCommand(booking.Id);

        // Act
        await handler.Handle(command);

        // Assert
        bookingRepositoryMock.Verify(r => r.SaveAsync(), Times.Once);
    }

    [Fact]
    public async Task Handle_GivenUnknownBookingId_CastNotFoundException()
    {
        // Arrange
        var bookingRepositoryMock = new Mock<IBookingRepository>();

        bookingRepositoryMock
            .Setup(repository => repository.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Booking?)null);

        ICancelBookingHandler handler = new CancelBookingHandler(bookingRepositoryMock.Object);

        var command = new CancelBookingCommand(Guid.NewGuid());

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(command));
    }
}