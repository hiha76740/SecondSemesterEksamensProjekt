using BookRight.DomainLib.Exceptions;
using BookRight.DomainLib.ValueObjects;

namespace BookRight.DomainLib.Tests.ValueObjects;

public class TimeSlotTests
{
    private static TimeSlot CreateTimeSlotWithValidData(
        DateTime? from = null,
        DateTime? to = null)
        => new TimeSlot(
            from ?? new DateTime(2026, 5, 1, 8, 0, 0),
            to ?? new DateTime(2026, 5, 1, 9, 0, 0));

    // ---------------------------------------------------------
    // 1. Create tests (Creating a TimeSlot)
    // ---------------------------------------------------------

    [Fact]
    public void Create_GivenFromAfterTo_CastDomainException()
    {
        // Arrange
        DateTime to = new DateTime(2026, 5, 2, 9, 0, 0);
        DateTime from = to.AddMinutes(30);

        // Act & Assert
        Assert.Throws<DomainException>(() =>
            CreateTimeSlotWithValidData(from, to));
    }

    [Fact]
    public void Create_GivenSameTime_CastDomainException()
    {
        // Arrange
        DateTime time = new DateTime(2026, 5, 2, 9, 0, 0);

        // Act & Assert
        Assert.Throws<DomainException>(() =>
            CreateTimeSlotWithValidData(time, time));
    }

    // ---------------------------------------------------------
    // 2. Duration tests (Calculating duration)
    // ---------------------------------------------------------

    [Fact]
    public void Duration_GivenValidFromAndTo_ReturnsCorrectDuration()
    {
        // Arrange
        DateTime from = new DateTime(2026, 5, 1, 9, 0, 0);
        DateTime to = new DateTime(2026, 5, 1, 9, 45, 0);

        TimeSlot timeSlot = CreateTimeSlotWithValidData(from, to);

        // Act
        TimeSpan duration = timeSlot.Duration;

        // Assert
        Assert.Equal(TimeSpan.FromMinutes(45), duration);
    }
}