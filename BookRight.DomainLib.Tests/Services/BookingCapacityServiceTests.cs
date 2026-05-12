using BookRight.DomainLib.Entities.Bookings;
using BookRight.DomainLib.Entities.Clinics;
using BookRight.DomainLib.Enums;
using BookRight.DomainLib.Services;
using BookRight.DomainLib.ValueObjects;

namespace BookRight.DomainLib.Tests.Services;

public class BookingCapacityServiceTests
{

    private static List<OpeningHourInput> OpeningHours = new()
    {
        new OpeningHourInput(
                Weekdays.Monday,
                new TimeOnly(8, 0, 0),
                new TimeOnly(16, 0, 0),
                false),

        new OpeningHourInput(
                Weekdays.Tuesday,
                new TimeOnly(8, 0, 0),
                new TimeOnly(16, 0, 0),
                false),

        new OpeningHourInput(
                Weekdays.Wednesday,
                new TimeOnly(8, 0, 0),
                new TimeOnly(16, 0, 0),
                false),

        new OpeningHourInput(
                Weekdays.Thursday,
                new TimeOnly(8, 0, 0),
                new TimeOnly(16, 0, 0),
                false),

        new OpeningHourInput(
                Weekdays.Friday,
                new TimeOnly(8, 0, 0),
                new TimeOnly(16, 0, 0),
                false),

        new OpeningHourInput(
                Weekdays.Saturday,
                new TimeOnly(8, 0, 0),
                new TimeOnly(16, 0, 0),
                false),

        new OpeningHourInput(
                Weekdays.Sunday,
                new TimeOnly(8, 0, 0),
                new TimeOnly(16, 0, 0),
                false)

    };
    private static int ClinicRoomLimit => 2;
    private static Clinic Clinic => Clinic.Create("TestKlinik", ClinicRoomLimit, OpeningHours, new Address("Testvej 1", "1234", "FantasiBy"));

    private static Booking CreateWithoutOverlap(DateTime from, DateTime to)
    {
        return Booking.Create(
            new TimeSlot(from, to),
            Guid.NewGuid(),
            Guid.NewGuid(),
            Guid.NewGuid(),
            500,
            1,
            DiscountTypes.None,
            Guid.NewGuid());
    }

    // ------------------------------------------------------------------
    // 1. CanCreateBooking tests (Checks if a booking can be created )
    // ------------------------------------------------------------------

    [Fact]
    public void CanCreateBooking_GivenOverlappingBookingsAreBelowRoomLimit_ReturnsTrue()
    {
        // Arrange
        IBookingCapacityService service = new BookingCapacityService();

        TimeSlot newTimeSlot = new TimeSlot(
            new DateTime(2027, 5, 1, 10, 0, 0),
            new DateTime(2027, 5, 1, 11, 0, 0));

        var existingBookings = new List<Booking>
        {
            CreateWithoutOverlap(
                new DateTime(2027,5,1,10,30,0),
                new DateTime(2027,5,1,11,30,0))
        };

        // Act
        var result = service.CanCreateBooking(Clinic, existingBookings, newTimeSlot);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void CanCreateBooking_GivenOverlappingBookingsAreAboveRoomLimit_ReturnsFalse()
    {
        // Arrange
        IBookingCapacityService service = new BookingCapacityService();

        TimeSlot newTimeSlot = new TimeSlot(
            new DateTime(2027, 5, 1, 10, 0, 0),
            new DateTime(2027, 5, 1, 11, 0, 0));

        var existingBookings = new List<Booking>
        {
            CreateWithoutOverlap(
                new DateTime(2027,5,1,10,20,0),
                new DateTime(2027,5,1,11,20,0)),

            CreateWithoutOverlap(
                new DateTime(2027,5,1,10,20,0),
                new DateTime(2027,5,1,11,20,0)),

            CreateWithoutOverlap(
                new DateTime(2027,5,1,10,10,0),
                new DateTime(2027,5,1,11,10,0))
        };

        // Act
        var result = service.CanCreateBooking(Clinic, existingBookings, newTimeSlot);

        // Assert
        Assert.False(result);
    }
}
