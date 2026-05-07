using BookRight.ApplicationLib.Handlers.Booking;
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
    /* 
     TODO:
     Ikke nødvendigt, du skal ikke ændre i parameterne i handler test, hvorfor du blot kan hard code dem ind,
     som jeg har gjort for dig i denne.
     

    private static Guid CustomerId => Guid.Parse("4504e34a-67a5-4cba-b029-8eb0b453b80d");
    private static Guid TreatmentId => Guid.Parse("4504e34a-67a5-4cba-b029-8eb0b693b80d");
    private static Guid TherapistId => Guid.Parse("4504e34a-67a5-4cba-b029-8eb0b993b80d");
    private static Guid ClinicId => Guid.Parse("4504e34a-67a5-4cba-b029-8eb0b493c80d");

    private static Guid UnknownBookingId => Guid.Parse("4504e34a-67a5-4cba-b029-8eb0b493b81d");

    private static decimal Price => 550m;

    private static IEnumerable<Booking> ExistingCustomerBookings => Array.Empty<Booking>();
    private static IEnumerable<Booking> ExistingTherapistBookings => Array.Empty<Booking>();
    */
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

    // ---------------------------------------------------------
    // 1. Handle tests (Cancelling a Booking through Application)
    // ---------------------------------------------------------

    /*
    
    TODO: Slettes, Vi tester kun 1 ting, ellers bryder du S i SOLID


    [Fact]
    public async Task Handle_GivenValidBookingId_SetsStatusToCancelled()
    {
        // Arrange
        Booking booking = CreateBooking();

        var bookingRepositoryMock = new Mock<IBookingRepository>();

        bookingRepositoryMock
            .Setup(repository => repository.GetByIdAsync(booking.Id))
            .ReturnsAsync(booking);
        
          Ikke nødvendig
        //bookingRepositoryMock
        //    .Setup(repository => repository.Save())
        //    .Returns(Task.CompletedTask);
        
        ICancelBookingHandler handler = new CancelBookingHandler(bookingRepositoryMock.Object);

        var command = new CancelBookingCommand(booking.Id);

        // Act
        await handler.Handle(command);

        // Assert
        Assert.Equal(BookingStatus.Cancelled, booking.Status);
    }
    */

    [Fact]
    public async Task Handle_GivenValidBookingId_CallsSave()
    {
        // Arrange
        Booking booking = CreateBooking();

        var bookingRepositoryMock = new Mock<IBookingRepository>();

        bookingRepositoryMock
            .Setup(repository => repository.GetByIdAsync(booking.Id))
            .ReturnsAsync(booking);

        /* 
         
        TODO: Dette er det du tester til slut med Verify, så det er blot fyld

        //bookingRepositoryMock
        //    .Setup(repository => repository.Save())
        //    .Returns(Task.CompletedTask);
        */

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
        await Assert.ThrowsAsync<Exceptions.ApplicationException>(() => handler.Handle(command));

        /*
        TODO:  
        Dette behøves ikke testes, der sker ikke når programmet får en Exception (normal C# kode)

        //bookingRepositoryMock.Verify(repository => repository.GetByIdAsync(It.IsAny<Guid>()), Times.Never);

        //bookingRepositoryMock.Verify(repository => repository.Save(), Times.Never);
        */
    }

    [Fact]
    public async Task Handle_GivenUnknownBookingId_CastNotFoundException()
    {
        // Arrange
        var bookingRepositoryMock = new Mock<IBookingRepository>();

        bookingRepositoryMock
            .Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Booking?)null);

        ICancelBookingHandler handler = new CancelBookingHandler(bookingRepositoryMock.Object);

        var command = new CancelBookingCommand(Guid.NewGuid());

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(command));

        /*
        TODO:  
        Dette behøves ikke testes, der sker ikke når programmet får en Exception (normal C# kode)

        //bookingRepositoryMock.Verify(repository => repository.Save(), Times.Never);
        */
    }
}