using BookRight.ApplicationLib.Handlers.Bookings;
using BookRight.ApplicationLib.Repositories;
using BookRight.DomainLib.Entities.Bookings;
using BookRight.DomainLib.Entities.Customers;
using BookRight.DomainLib.Entities.Therapists;
using BookRight.DomainLib.Enums;
using BookRight.DomainLib.Exceptions;
using BookRight.DomainLib.Services;
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
            Guid.Parse("4504e34a-67a5-4cba-b029-8eb0b693b80d"),
            Guid.Parse("4504e34a-67a5-4cba-b029-8eb0b993b80d"),
            Guid.Parse("4504e34a-67a5-4cba-b029-8eb0b493c80d"),
            550m,
            1,
            DiscountTypes.None, 
            Guid.Parse("4504e34a-67a5-4cba-b029-8eb0b453b80d"));
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

        var mockValidateOverlapService = new Mock<IValidateOverlapService>();
        var bookingRepositoryMock = new Mock<IBookingRepository>();
        var customerRepositoryMock = new Mock<ICustomerRepository>();
        var therapistRepositoryMock = new Mock<ITherapistRepository>();

        bookingRepositoryMock
            .Setup(r => r.GetByIdAsync(booking.Id))
            .ReturnsAsync(booking);

        bookingRepositoryMock
            .Setup(r => r.GetAllBookingsByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(new List<Booking>());

        customerRepositoryMock
            .Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(CreateCustomer());

        therapistRepositoryMock
            .Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(CreateTherapist());

        IChangeTimeHandler handler = new ChangeTimeHandler(
            bookingRepositoryMock.Object,
            customerRepositoryMock.Object,
            therapistRepositoryMock.Object,
            mockValidateOverlapService.Object);

        TimeSlot newTimeSlot = CreateTimeSlot(12, 13);

        var command = new ChangeTimeCommand(
            booking.Id,
            newTimeSlot.From,
            newTimeSlot.To);

        // Act
        await handler.Handle(command);

        // Assert
        bookingRepositoryMock.Verify(r => r.SaveAsync(), Times.Once);
    }

    [Fact]
    public async Task Handle_GivenUnknownBookingId_CastNotFoundException()
    {
        // Arrange
        var mockValidateOverlapService = new Mock<IValidateOverlapService>();
        var bookingRepositoryMock = new Mock<IBookingRepository>();
        var customerRepositoryMock = new Mock<ICustomerRepository>();
        var therapistRepositoryMock = new Mock<ITherapistRepository>();

        bookingRepositoryMock
            .Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Booking?)null);

        IChangeTimeHandler handler = new ChangeTimeHandler(
            bookingRepositoryMock.Object,
            customerRepositoryMock.Object,
            therapistRepositoryMock.Object, 
            mockValidateOverlapService.Object);

        TimeSlot newTimeSlot = CreateTimeSlot(12, 13);

        var command = new ChangeTimeCommand(
            Guid.NewGuid(),
            newTimeSlot.From,
            newTimeSlot.To);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(command));
    }
}