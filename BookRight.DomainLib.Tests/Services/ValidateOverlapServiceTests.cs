using BookRight.DomainLib.Entities.Bookings;
using BookRight.DomainLib.Enums;
using BookRight.DomainLib.Exceptions;
using BookRight.DomainLib.Services;
using BookRight.DomainLib.ValueObjects;

namespace BookRight.DomainLib.Tests.Services;

public class ValidateOverlapServiceTests
{
    private static Booking CreateWithoutOverlap(TimeSlot? time = null)
    {
        return Booking.Create(
            time ?? new TimeSlot(
                new DateTime(2027, 05, 01, 09, 00, 00), 
            new DateTime(2027, 05, 01, 11, 00, 00)),
            Guid.NewGuid(),
            Guid.NewGuid(),
            Guid.NewGuid(),
            500,
            1,
            DiscountTypes.None,
            Guid.NewGuid());
    }

    [Theory]
    [InlineData("2027-05-01 09:30", "2027-05-01 10:30")]  // overlapper inde i
    [InlineData("2027-05-01 08:00", "2027-05-01 09:30")]  // overlapper starten
    [InlineData("2027-05-01 10:30", "2027-05-01 11:30")]  // overlapper slutningen
    public void Create_GivenOverlaps_CastDomainException(string otherFromText, string otherToText)
    {
        // Arrange
        var booking = CreateWithoutOverlap();

        var other = new TimeSlot(
            DateTime.Parse(otherFromText),
            DateTime.Parse(otherToText));

        var service = new ValidateOverlapService() as IValidateOverlapService;

        var exsist = CreateWithoutOverlap(other);

        // Act & Assert
        Assert.Throws<DomainException>(() => service.Validate(booking, new[] { exsist }));
    }
}
