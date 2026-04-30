using BookRight.DomainLib.Exceptions;

namespace BookRight.DomainLib.ValueObjects;

public record OpeningHours
{
    public DateTime Open { get; init; }

    public DateTime Close { get; init; }

    public OpeningHours(DateTime open, DateTime close)
    {
        if (open > close)
            throw new DomainException("Closing time can not be after opening hour");

        if (open.Day != close.Day)
            throw new DomainException("Open and close time must be on the same day");

        Open = open;
        Close = close;
    }
}
