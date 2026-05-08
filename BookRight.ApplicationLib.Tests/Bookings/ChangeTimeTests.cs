using BookRight.ApplicationLib.Handlers.Booking;
using BookRight.ApplicationLib.Repositories;
using BookRight.DomainLib.Entities.Bookings;
using BookRight.DomainLib.Entities.Customers;
using BookRight.DomainLib.Entities.Therapists;
using BookRight.DomainLib.Enums;
using BookRight.DomainLib.Exceptions;
using BookRight.DomainLib.ValueObjects;
using BookRight.FacadeLib.Commands.Booking.DTOs;
using BookRight.FacadeLib.Commands.Booking.Interfaces;
using Moq;

namespace BookRight.ApplicationLib.Tests.Bookings;

public class ChangeTimeTests
{
    private static TimeSlot CreateTimeSlot(int fromHour, int toHour)
    {
        return new TimeSlot(
            DateTime.UtcNow.AddDays(1).Date.AddHours(fromHour),
            DateTime.UtcNow.AddDays(1).Date.AddHours(toHour));
    }

    private static Booking CreateBooking(TimeSlot? timeSlot = null)
    {
        return Booking.Create(
            timeSlot ?? CreateTimeSlot(9, 10),
            Guid.Parse("4504e34a-67a5-4cba-b029-8eb0b453b80d"),
            Guid.Parse("4504e34a-67a5-4cba-b029-8eb0b693b80d"),
            Guid.Parse("4504e34a-67a5-4cba-b029-8eb0b993b80d"),
            Guid.Parse("4504e34a-67a5-4cba-b029-8eb0b493c80d"),
            550m,
            Array.Empty<Booking>(),
            Array.Empty<Booking>());
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

    private static Therapist CreateTherapist()
    {
        return Therapist.Create(
            "AUTH123",
            "Test Therapist",
            500m,
            new Address("Testvej 2", "7100", "Vejle"),
            new Email("therapist@test.dk"),
            new PhoneNumber("87654321"),
            new List<Guid> { Guid.Parse("4504e34a-67a5-4cba-b029-8eb0b493c80d") },
            new List<CertificationTypes> { CertificationTypes.Physiotherapy });
    }

    // ---------------------------------------------------------
    // 1. Handle tests (Changing time through Application)
    // ---------------------------------------------------------

    [Fact]
    public async Task Handle_GivenValidBookingId_CallsSave()
    {
        // Arrange
        Booking booking = CreateBooking();

        var bookingRepositoryMock = new Mock<IBookingRepository>();
        var customerRepositoryMock = new Mock<ICustomerRepository>();
        var therapistRepositoryMock = new Mock<ITherapistRepository>();

        bookingRepositoryMock
            .Setup(repository => repository.GetByIdAsync(booking.Id))
            .ReturnsAsync(booking);

        bookingRepositoryMock
            .Setup(repository => repository.GetAllBookingsByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(new List<Booking>());

        customerRepositoryMock
            .Setup(repository => repository.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(CreateCustomer());

        therapistRepositoryMock
            .Setup(repository => repository.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(CreateTherapist());

        IChangeTimeHandler handler = new ChangeTimeHandler(
            bookingRepositoryMock.Object,
            customerRepositoryMock.Object,
            therapistRepositoryMock.Object);

        TimeSlot newTimeSlot = CreateTimeSlot(12, 13);

        var command = new ChangeTimeCommand(
            booking.Id,
            newTimeSlot.From,
            newTimeSlot.To);

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
        var customerRepositoryMock = new Mock<ICustomerRepository>();
        var therapistRepositoryMock = new Mock<ITherapistRepository>();

        IChangeTimeHandler handler = new ChangeTimeHandler(
            bookingRepositoryMock.Object,
            customerRepositoryMock.Object,
            therapistRepositoryMock.Object);

        TimeSlot newTimeSlot = CreateTimeSlot(12, 13);

        var command = new ChangeTimeCommand(
            Guid.Empty,
            newTimeSlot.From,
            newTimeSlot.To);

        // Act & Assert
        await Assert.ThrowsAsync<Exceptions.ApplicationException>(() => handler.Handle(command));
    }

    [Fact]
    public async Task Handle_GivenUnknownBookingId_CastNotFoundException()
    {
        // Arrange
        var bookingRepositoryMock = new Mock<IBookingRepository>();
        var customerRepositoryMock = new Mock<ICustomerRepository>();
        var therapistRepositoryMock = new Mock<ITherapistRepository>();

        bookingRepositoryMock
            .Setup(repository => repository.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Booking?)null);

        IChangeTimeHandler handler = new ChangeTimeHandler(
            bookingRepositoryMock.Object,
            customerRepositoryMock.Object,
            therapistRepositoryMock.Object);

        TimeSlot newTimeSlot = CreateTimeSlot(12, 13);

        var command = new ChangeTimeCommand(
            Guid.NewGuid(),
            newTimeSlot.From,
            newTimeSlot.To);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(command));
    }

    [Fact]
    public async Task Handle_GivenTimeOverlap_CastDomainException()
    {
        // Arrange
        TimeSlot newTimeSlot = CreateTimeSlot(12, 13);

        Booking booking = CreateBooking();
        Booking existingBooking = CreateBooking(newTimeSlot);

        var bookingRepositoryMock = new Mock<IBookingRepository>();
        var customerRepositoryMock = new Mock<ICustomerRepository>();
        var therapistRepositoryMock = new Mock<ITherapistRepository>();

        bookingRepositoryMock
            .Setup(repository => repository.GetByIdAsync(booking.Id))
            .ReturnsAsync(booking);

        bookingRepositoryMock
            .Setup(repository => repository.GetAllBookingsByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(new List<Booking> { existingBooking });

        customerRepositoryMock
            .Setup(repository => repository.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(CreateCustomer());

        therapistRepositoryMock
            .Setup(repository => repository.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(CreateTherapist());

        IChangeTimeHandler handler = new ChangeTimeHandler(
            bookingRepositoryMock.Object,
            customerRepositoryMock.Object,
            therapistRepositoryMock.Object);

        var command = new ChangeTimeCommand(
            booking.Id,
            newTimeSlot.From,
            newTimeSlot.To);

        // Act & Assert
        await Assert.ThrowsAsync<DomainException>(() => handler.Handle(command));
    }
}