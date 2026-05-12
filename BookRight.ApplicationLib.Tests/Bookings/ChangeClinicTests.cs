using BookRight.ApplicationLib.Handlers.Bookings;
using BookRight.ApplicationLib.Repositories;
using BookRight.DomainLib.Entities.Bookings;
using BookRight.DomainLib.Entities.Clinics;
using BookRight.DomainLib.Enums;
using BookRight.DomainLib.Exceptions;
using BookRight.DomainLib.Services;
using BookRight.DomainLib.ValueObjects;
using BookRight.FacadeLib.Commands.Booking.DTOs;
using BookRight.FacadeLib.Commands.Booking.Interfaces;
using Moq;

namespace BookRight.ApplicationLib.Tests.Bookings;

public class ChangeClinicTests
{
    private static List<OpeningHourInput> OpeningHours = new()
    {
        new OpeningHourInput(
                WeekDays.Monday,
                new TimeOnly(8, 0, 0),
                new TimeOnly(16, 0, 0),
                false),

        new OpeningHourInput(
                WeekDays.Tuesday,
                new TimeOnly(8, 0, 0),
                new TimeOnly(16, 0, 0),
                false),

        new OpeningHourInput(
                WeekDays.Wednesday,
                new TimeOnly(8, 0, 0),
                new TimeOnly(16, 0, 0),
                false),

        new OpeningHourInput(
                WeekDays.Thursday,
                new TimeOnly(8, 0, 0),
                new TimeOnly(16, 0, 0),
                false),

        new OpeningHourInput(
                WeekDays.Friday,
                new TimeOnly(8, 0, 0),
                new TimeOnly(16, 0, 0),
                false),

        new OpeningHourInput(
                WeekDays.Saturday,
                new TimeOnly(8, 0, 0),
                new TimeOnly(16, 0, 0),
                false),

        new OpeningHourInput(
                WeekDays.Sunday,
                new TimeOnly(8, 0, 0),
                new TimeOnly(16, 0, 0),
                false)

    };

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
            DiscountTypes.None,
            Guid.Parse("4504e34a-67a5-4cba-b029-8eb0b453b80d"));
    }

    private static Clinic CreateClinic()
    {
        return Clinic.Create(
            "Klinik Vejle",
            5,
            OpeningHours,
            new Address("Testvej 1", "7100", "Vejle"));
    }

    // ---------------------------------------------------------
    // 1. Handle tests (Changing clinic through Application)
    // ---------------------------------------------------------

    [Fact]
    public async Task Handle_GivenValidBookingId_CallsSave()
    {
        // Arrange
        Booking booking = CreateBooking();
        Clinic clinic = CreateClinic();

        var bookingRepositoryMock = new Mock<IBookingRepository>();
        var clinicRepositoryMock = new Mock<IClinicRepository>();
        var bookingCapacityServiceMock = new Mock<IBookingCapacityService>();

        bookingRepositoryMock
            .Setup(r => r.GetByIdAsync(booking.Id))
            .ReturnsAsync(booking);

        clinicRepositoryMock
            .Setup(r => r.GetByIdAsync(clinic.Id))
            .ReturnsAsync(clinic);

        bookingRepositoryMock
            .Setup(r => r.GetAllBookingsByIdAsync(clinic.Id))
            .ReturnsAsync(new List<Booking>());

        bookingCapacityServiceMock
            .Setup(s => s.CanCreateBooking(clinic, It.IsAny<IEnumerable<Booking>>(), booking.Time))
            .Returns(true);

        IChangeClinicHandler handler = new ChangeClinicHandler(
            bookingRepositoryMock.Object,
            clinicRepositoryMock.Object,
            bookingCapacityServiceMock.Object);

        var command = new ChangeClinicCommand(
            booking.Id,
            clinic.Id);

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
        var clinicRepositoryMock = new Mock<IClinicRepository>();
        var bookingCapacityServiceMock = new Mock<IBookingCapacityService>();

        bookingRepositoryMock
            .Setup(repository => repository.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Booking?)null);

        IChangeClinicHandler handler = new ChangeClinicHandler(
            bookingRepositoryMock.Object,
            clinicRepositoryMock.Object,
            bookingCapacityServiceMock.Object);

        var command = new ChangeClinicCommand(
            Guid.NewGuid(),
            Guid.NewGuid());

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(command));
    }

    [Fact]
    public async Task Handle_GivenUnknownClinicId_CastNotFoundException()
    {
        // Arrange
        Booking booking = CreateBooking();

        var bookingRepositoryMock = new Mock<IBookingRepository>();
        var clinicRepositoryMock = new Mock<IClinicRepository>();
        var bookingCapacityServiceMock = new Mock<IBookingCapacityService>();

        bookingRepositoryMock
            .Setup(repository => repository.GetByIdAsync(booking.Id))
            .ReturnsAsync(booking);

        clinicRepositoryMock
            .Setup(repository => repository.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Clinic?)null);

        IChangeClinicHandler handler = new ChangeClinicHandler(
            bookingRepositoryMock.Object,
            clinicRepositoryMock.Object,
            bookingCapacityServiceMock.Object);

        var command = new ChangeClinicCommand(
            booking.Id,
            Guid.NewGuid());

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(command));
    }

    [Fact]
    public async Task Handle_GivenNoClinicCapacity_CastApplicationException()
    {
        // Arrange
        Booking booking = CreateBooking();
        Clinic clinic = CreateClinic();

        var bookingRepositoryMock = new Mock<IBookingRepository>();
        var clinicRepositoryMock = new Mock<IClinicRepository>();
        var bookingCapacityServiceMock = new Mock<IBookingCapacityService>();

        bookingRepositoryMock
            .Setup(r => r.GetByIdAsync(booking.Id))
            .ReturnsAsync(booking);

        clinicRepositoryMock
            .Setup(r => r.GetByIdAsync(clinic.Id))
            .ReturnsAsync(clinic);

        bookingRepositoryMock
            .Setup(r => r.GetAllBookingsByIdAsync(clinic.Id))
            .ReturnsAsync(new List<Booking>());

        bookingCapacityServiceMock
            .Setup(s => s.CanCreateBooking(clinic, It.IsAny<IEnumerable<Booking>>(), booking.Time))
            .Returns(false);

        IChangeClinicHandler handler = new ChangeClinicHandler(
            bookingRepositoryMock.Object,
            clinicRepositoryMock.Object,
            bookingCapacityServiceMock.Object);

        var command = new ChangeClinicCommand(
            booking.Id,
            clinic.Id);

        // Act & Assert
        await Assert.ThrowsAsync<Exceptions.ApplicationException>(() => handler.Handle(command));
    }
}