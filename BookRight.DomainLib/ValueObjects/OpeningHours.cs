using BookRight.DomainLib.Exceptions;

namespace BookRight.DomainLib.ValueObjects;

/// <summary>
/// Represents the opening and closing hours for a single day period.
/// </summary>
/// <remarks>The opening and closing times must be on the same day, and the closing time must be after the opening
/// time. This record is immutable and is typically used to specify business or service availability within a single
/// calendar day.</remarks>
public record OpeningHours
{
    public DateTime Open { get; init; }

    public DateTime Close { get; init; }

    public OpeningHours(DateTime open, DateTime close)
    {
        if (open >= close)
            throw new DomainException("Closing time can not be same or before opening hour");

        if (open.Day != close.Day)
            throw new DomainException("Open and close time must be on the same day");

        Open = open;
        Close = close;
    }
}
