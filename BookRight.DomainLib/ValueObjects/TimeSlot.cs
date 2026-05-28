using BookRight.DomainLib.Exceptions;

namespace BookRight.DomainLib.ValueObjects;

//
public record TimeSlot
{
    public DateTime From { get; init; }
    public DateTime To { get; init; }

    public TimeSlot(DateTime from, DateTime to)
    {
        if (to <= from)
            throw new DomainException("End time must be after start time.");

        From = from;
        To = to;
    }

    public TimeSpan Duration => To - From;

    /// <summary>
    /// Determines whether the time slot overlaps with another time slot.
    /// </summary>
    /// <remarks>Slots are treated as half-open intervals (start inclusive, end exclusive). Intervals that
    /// only touch at their boundaries do not overlap.</remarks>
    /// <param name="other">The other time slot to compare against.</param>
    /// <returns>true if the time slots overlap; otherwise, false.</returns>
    internal bool OverlapsWith(TimeSlot other) => From < other.To && other.From < To;

    // Parameterløs constructor til EF Core
    private TimeSlot() { }
}
